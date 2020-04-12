/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.IO;
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
        readonly INativeCalls api;
        readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        readonly Thread thread;
        readonly ConsoleOutputModes originalOutputMode;
        readonly ConsoleInputModes originalInputMode;

        readonly SafeFileHandle readStdOutHandle;
        readonly SafeFileHandle writeStdOutHandle;
        readonly SafeFileHandle readErrorHandle;
        readonly SafeFileHandle writeErrorHandle;
        readonly FileStream stdoutStream;
        readonly FileStream errorStream;
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

            this.api.CreatePipe(out readStdOutHandle, out writeStdOutHandle);
            using (var tempHandle = new ConsoleOutputHandle(writeStdOutHandle.DangerousGetHandle()))
                this.api.SetOutputHandle(tempHandle);
            stdoutStream = new FileStream(readStdOutHandle, FileAccess.Read);
            this.api.CreatePipe(out readErrorHandle, out writeErrorHandle);
            using (var tempHandle = new ConsoleErrorHandle(writeErrorHandle.DangerousGetHandle()))
                this.api.SetErrorHandle(tempHandle);
            errorStream = new FileStream(readErrorHandle, FileAccess.Read);

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
            stopEvent.Set();
            thread.Join();
            stopEvent.Dispose();
            api.SetErrorHandle(OriginalErrorHandle);
            api.SetOutputHandle(OriginalOutputHandle);
            api.SetConsoleMode(OriginalInputHandle, originalInputMode);
            api.SetConsoleMode(OriginalOutputHandle, originalOutputMode);
            errorStream.Dispose();
            stdoutStream.Dispose();
            readStdOutHandle.Dispose();
            writeStdOutHandle.Dispose();
            readErrorHandle.Dispose();
            writeErrorHandle.Dispose();
            OriginalOutputHandle.Dispose();
            OriginalErrorHandle.Dispose();
            OriginalInputHandle.Dispose();
        }
        void ListenerThread()
        {
            try
            {
                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Starting thread.");
                StartReadingStdout();
                StartReadingError();
                IntPtr stdin = OriginalOutputHandle.DangerousGetHandle();
                using var inputWaitHandle = new AutoResetEvent(false) {SafeWaitHandle = new SafeWaitHandle(stdin, false)};
                WaitHandle[] waitHandles = {stopEvent, inputWaitHandle};
                int index;
                while ((index = WaitHandle.WaitAny(waitHandles)) != 0)
                {
                    switch (index)
                    {
                        case 1:
                            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Input handle signaled.");
                            ReadConsoleInput();
                            break;
                    }
                }

                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Stopping thread.");
            }
            catch (Exception e)
            {
                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener | DebugContext.Exception, $"Thread failed: {e}");
                throw;
            }
        }
        void ReadConsoleInput()
        {
            try
            {
                var records = api.ReadConsoleInput(OriginalInputHandle);
                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, $"Read {records.Length} input records.");
                foreach (var record in records)
                {
                    Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, $"Record of type {record.EventType}.");
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
                            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, $"Unkown input record type \"{record.EventType}\"!");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, e.ToString());
                throw;
            }
        }
        void StartReadingStdout() =>
            stdoutStream.BeginRead(stdoutBuffer, 0, stdoutBuffer.Length, OnStdoutRead, null);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031", Justification = "Catch client handler exceptions.")]
        void OnStdoutRead(IAsyncResult ar)
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Received stdout signal.");
            int read = stdoutStream.EndRead(ar);
            StartReadingStdout();
            if (stopEvent.WaitOne(0)) return;
            string msg = Encoding.Default.GetString(stdoutBuffer, 0, read);
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, $"Read {read} bytes from stdout: [{msg}]");
            OutputReceived?.Invoke(this, new ConsoleOutputReceivedEventArgs(msg));
        }
        void StartReadingError() =>
            errorStream.BeginRead(errorBuffer, 0, errorBuffer.Length, OnErrorRead, null);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031", Justification = "Catch client handler exceptions.")]
        void OnErrorRead(IAsyncResult ar)
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Received error signal.");
            int read = errorStream.EndRead(ar);
            StartReadingError();
            if (stopEvent.WaitOne(0)) return;
            string msg = Encoding.Default.GetString(errorBuffer, 0, read);
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, $"Read {read} bytes from stderr: [{msg}]");
            ErrorReceived?.Invoke(this, new ConsoleOutputReceivedEventArgs(msg));
        }
    }
}
