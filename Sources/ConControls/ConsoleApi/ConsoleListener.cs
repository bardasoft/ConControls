﻿/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
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

        int disposed;

        public event EventHandler<ConsoleOutputReceivedEventArgs>? OutputReceived;
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
            OriginalOutputHandle.Dispose();
            api.SetErrorHandle(OriginalErrorHandle);
            api.SetOutputHandle(OriginalOutputHandle);
            api.SetConsoleMode(OriginalInputHandle, originalInputMode);
            api.SetConsoleMode(OriginalOutputHandle, originalOutputMode);
            OriginalOutputHandle.Dispose();
        }
        void ListenerThread()
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Starting thread.");
            IntPtr stdin = OriginalOutputHandle.DangerousGetHandle();
            using var inputWaitHandle = new AutoResetEvent(false) {SafeWaitHandle = new SafeWaitHandle(stdin, false)};
            WaitHandle[] waitHandles = {stopEvent, inputWaitHandle};
            int index;
            while ((index = WaitHandle.WaitAny(waitHandles)) != 0)
            {
                if (index != 1) continue;
                //inputWaitHandle.Reset();
                Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Input handle signaled.");
                ReadConsoleInput();
            }
            Logger.Log(DebugContext.ConsoleApi | DebugContext.ConsoleListener, "Stopping thread.");
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
    }
}
