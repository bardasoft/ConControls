/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
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
        readonly ConsoleInputHandle consoleInputHandle = new ConsoleInputHandle();
        readonly ConsoleOutputHandle consoleOutputHandle = new ConsoleOutputHandle();
        int disposed;

        public event EventHandler<ConsoleOutputReceivedEventArgs>? OutputReceived;
        public event EventHandler<ConsoleFocusEventArgs>? FocusEvent;
        public event EventHandler<ConsoleKeyEventArgs>? KeyEvent;
        public event EventHandler<ConsoleMouseEventArgs>? MouseEvent;
        public event EventHandler<ConsoleSizeEventArgs>? SizeEvent;
        public event EventHandler<ConsoleMenuEventArgs>? MenuEvent;

        internal ConsoleListener(INativeCalls? api = null)
        {
            this.api = api ?? new NativeCalls();
            this.api.SetConsoleMode(consoleInputHandle,
                                    ConsoleInputModes.EnableWindowInput |
                                    ConsoleInputModes.EnableMouseInput |
                                    ConsoleInputModes.EnableExtendedFlags);
            this.api.SetConsoleMode(consoleOutputHandle, ConsoleOutputModes.None);

            thread = new Thread(ListenerThread);
            thread.Start();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0) return;
            stopEvent.Set();
            thread.Join();
            stopEvent.Dispose();
            consoleInputHandle.Dispose();
            consoleOutputHandle.Dispose();
        }
        void ListenerThread()
        {
            Log("Starting thread.");
            IntPtr stdin = consoleInputHandle.DangerousGetHandle();
            using var inputWaitHandle = new AutoResetEvent(false) {SafeWaitHandle = new SafeWaitHandle(stdin, false)};
            WaitHandle[] waitHandles = {stopEvent, inputWaitHandle};
            int index;
            while ((index = WaitHandle.WaitAny(waitHandles)) != 0)
            {
                if (index != 1) continue;
                //inputWaitHandle.Reset();
                Log("Input handle signaled.");
                ReadConsoleInput();
            }
            Log("Stopping thread.");
        }
        void ReadConsoleInput()
        {
            try
            {
                var records = api.ReadConsoleInput(consoleInputHandle);
                Log($"Read {records.Length} input records.");
                foreach (var record in records)
                {
                    Log($"Record of type {record.EventType}.");
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
                            Log($"Unkown input record type \"{record.EventType}\"!");
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Log(e.ToString());
                throw;
            }
        }
        [Conditional("DEBUG")]
        static void Log(string msg, [CallerMemberName] string member = "?")
        {
            Debug.WriteLine($"{nameof(ConsoleListener)}.{member}: {msg}");
        }
    }
}
