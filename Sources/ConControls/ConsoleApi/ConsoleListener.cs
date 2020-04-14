/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using ConControls.Logging;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;
using Microsoft.Win32.SafeHandles;

namespace ConControls.ConsoleApi
{
    sealed class ConsoleListener : IConsoleListener
    {
        const DebugContext dbgctx = DebugContext.ConsoleApi | DebugContext.ConsoleListener;
        readonly object syncLock = new object();
        readonly INativeCalls api;
        readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        readonly Thread thread;
        readonly ConsoleOutputModes originalOutputMode;
        readonly ConsoleInputModes originalInputMode;

        readonly AnonymousPipeServerStream stdoutWriteStream;
        readonly AnonymousPipeClientStream stdoutReadStream;
        readonly AnonymousPipeServerStream stderrWriteStream;
        readonly AnonymousPipeClientStream stderrReadStream;
        readonly byte[] stdoutBuffer = new byte[2048];
        readonly byte[] errorBuffer = new byte[2048];

        int disposed;

        public event EventHandler<ConsoleOutputReceivedEventArgs>? OutputReceived;
        public event EventHandler<ConsoleOutputReceivedEventArgs>? ErrorReceived;
        public event EventHandler<ConsoleFocusEventArgs>? FocusEvent;
        public event EventHandler<ConsoleKeyEventArgs>? KeyEvent;
        public event EventHandler<ConsoleMouseEventArgs>? MouseEvent;
        public event EventHandler<ConsoleSizeEventArgs>? SizeEvent;
        public event EventHandler<ConsoleMenuEventArgs>? MenuEvent;

        public ConsoleErrorHandle OriginalErrorHandle { get; }
        public ConsoleInputHandle OriginalInputHandle { get; }
        public ConsoleOutputHandle OriginalOutputHandle { get; }

        internal ConsoleListener(INativeCalls? api = null)
        {
            this.api = api ?? new NativeCalls();
            OriginalErrorHandle = this.api.GetErrorHandle();
            OriginalOutputHandle = this.api.GetOutputHandle();
            originalOutputMode = this.api.GetConsoleMode(OriginalOutputHandle);
            OriginalInputHandle = this.api.GetInputHandle();
            originalInputMode = this.api.GetConsoleMode(OriginalInputHandle);

            stdoutWriteStream = new AnonymousPipeServerStream(PipeDirection.Out);
            stdoutReadStream = new AnonymousPipeClientStream(PipeDirection.In, stdoutWriteStream.ClientSafePipeHandle);
            using (var tempHandle = new ConsoleOutputHandle(stdoutWriteStream.SafePipeHandle.DangerousGetHandle()))
                this.api.SetOutputHandle(tempHandle);
            StartReadingStdout();

            stderrWriteStream = new AnonymousPipeServerStream(PipeDirection.Out);
            stderrReadStream = new AnonymousPipeClientStream(PipeDirection.In, stderrWriteStream.ClientSafePipeHandle);
            using (var tempHandle = new ConsoleErrorHandle(stderrWriteStream.SafePipeHandle.DangerousGetHandle()))
                this.api.SetErrorHandle(tempHandle);
            StartReadingError();

            this.api.SetConsoleMode(OriginalInputHandle,
                                    ConsoleInputModes.EnableWindowInput |
                                    ConsoleInputModes.EnableMouseInput |
                                    ConsoleInputModes.EnableExtendedFlags);
            this.api.SetConsoleMode(OriginalOutputHandle, ConsoleOutputModes.None);

            thread = new Thread(ListenerThread);
            thread.Start();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0) return;
            Logger.Log(dbgctx, "Disposing.");
            stopEvent.Set();
            api.SetErrorHandle(OriginalErrorHandle);
            api.SetOutputHandle(OriginalOutputHandle);
            lock (syncLock)
            {
                stdoutWriteStream.Dispose();
                stderrWriteStream.Dispose();
                stdoutReadStream.Dispose();
                stderrReadStream.Dispose();
            }

            thread.Join();
            Logger.Log(dbgctx, "Thread finally finished.");
            stopEvent.Dispose();
            api.SetConsoleMode(OriginalInputHandle, originalInputMode);
            api.SetConsoleMode(OriginalOutputHandle, originalOutputMode);
            OriginalOutputHandle.Dispose();
            OriginalErrorHandle.Dispose();
            OriginalInputHandle.Dispose();
        }

        [SuppressMessage("Design", "CA1031", Justification = "Leave thread cleanly.")]
        void ListenerThread()
        {
            try
            {
                Logger.Log(dbgctx, "Starting thread.");
                IntPtr stdin = OriginalInputHandle.DangerousGetHandle();
                using var inputWaitHandle = new AutoResetEvent(false) {SafeWaitHandle = new SafeWaitHandle(stdin, false)};
                WaitHandle[] waitHandles = {stopEvent, inputWaitHandle};
                int index;
                Logger.Log(dbgctx, "Waiting for input or stop event.");
                while ((index = WaitHandle.WaitAny(waitHandles)) != 0)
                {
                    Logger.Log(dbgctx, $"Signaled handle: {index}.");
                    switch (index)
                    {
                        case 1:
                            Logger.Log(dbgctx, "Input handle signaled.");
                            ReadConsoleInput();
                            break;
                    }
                }

                Logger.Log(dbgctx, "Stopping thread.");
            }
            catch (Exception e)
            {
                Logger.Log(dbgctx | DebugContext.Exception, $"Thread failed: {e}");
            }
        }
        void ReadConsoleInput()
        {
            try
            {
                var records = api.ReadConsoleInput(OriginalInputHandle);
                Logger.Log(dbgctx, $"Read {records.Length} input records.");
                foreach (var record in records)
                {
                    Logger.Log(dbgctx, $"Record of type {record.EventType}.");
                    switch (record.EventType)
                    {
                        case InputEventType.Key:
                            KeyEvent?.Invoke(this, new ConsoleKeyEventArgs(record.Event.KeyEvent));
                            break;
                        case InputEventType.Mouse:
                            MouseEvent?.Invoke(this, new ConsoleMouseEventArgs(record.Event.MouseEvent));
                            break;
                        case InputEventType.WindowBufferSize:
                            SizeEvent?.Invoke(this, new ConsoleSizeEventArgs(record.Event.SizeEvent));
                            break;
                        case InputEventType.Menu:
                            MenuEvent?.Invoke(this, new ConsoleMenuEventArgs(record.Event.MenuEent));
                            break;
                        case InputEventType.Focus:
                            FocusEvent?.Invoke(this, new ConsoleFocusEventArgs(record.Event.FocusEvent));
                            break;
                        default:
                            Logger.Log(dbgctx, $"Unkown input record type \"{record.EventType}\"!");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(dbgctx, e.ToString());
                throw;
            }
        }
        void StartReadingStdout()
        {
            Logger.Log(dbgctx, "Start reading from stdout.");
            stdoutReadStream.BeginRead(stdoutBuffer, 0, stdoutBuffer.Length, OnStdoutRead, null);
        }
        [SuppressMessage("Design", "CA1031", Justification = "Catch client handler exceptions.")]
        void OnStdoutRead(IAsyncResult ar)
        {
            int read;
            lock (syncLock)
            {
                Logger.Log(dbgctx, $"Received stdout signal ({(disposed > 0 ? "dead" : "alive")}).");

                if (disposed > 0) return;
                read = stdoutReadStream.EndRead(ar);
                if (read <= 0)
                {
                    Logger.Log(dbgctx, "Read zero bytes, stream seems closed!");
                    return;
                }

                StartReadingStdout();
            }

            string msg = Encoding.Default.GetString(stdoutBuffer, 0, read);
            Logger.Log(dbgctx, $"Read {read} bytes from stdout: [{msg}]");
            OutputReceived?.Invoke(this, new ConsoleOutputReceivedEventArgs(msg));
        }
        void StartReadingError()
        {
            stderrReadStream.BeginRead(errorBuffer, 0, errorBuffer.Length, OnErrorRead, null);
            Logger.Log(dbgctx, "Start reading from stderr.");
        }
        [SuppressMessage("Design", "CA1031", Justification = "Catch client handler exceptions.")]
        void OnErrorRead(IAsyncResult ar)
        {
            int read;
            lock (syncLock)
            {
                Logger.Log(dbgctx, $"Received error signal ({(disposed > 0 ? "dead" : "alive")}).");
                if (disposed > 0) return;
                read = stderrReadStream.EndRead(ar);
                if (read <= 0)
                {
                    Logger.Log(dbgctx, "Read zero bytes, stream seems closed!");
                    return;
                }

                StartReadingError();
            }

            if (stopEvent.WaitOne(0)) return;
            string msg = Encoding.Default.GetString(errorBuffer, 0, read);
            Logger.Log(dbgctx, $"Read {read} bytes from stderr: [{msg}]");
            ErrorReceived?.Invoke(this, new ConsoleOutputReceivedEventArgs(msg));
        }
    }
}
