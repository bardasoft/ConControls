/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using ConControls.Logging;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;
using Microsoft.Win32.SafeHandles;

namespace ConControls.ConsoleApi
{
    sealed class ConsoleController : IConsoleController
    {
        const DebugContext dbgctx = DebugContext.ConsoleApi | DebugContext.ConsoleListener;
        readonly INativeCalls api;
        readonly ManualResetEvent stopEvent = new ManualResetEvent(false);
        readonly ConsoleOutputHandle originalOutputHandle;
        readonly ConsoleInputModes originalInputMode;
        
        int disposed;

        public event EventHandler<ConsoleFocusEventArgs>? FocusEvent;
        public event EventHandler<ConsoleKeyEventArgs>? KeyEvent;
        public event EventHandler<ConsoleMouseEventArgs>? MouseEvent;
        public event EventHandler<ConsoleSizeEventArgs>? SizeEvent;
        public event EventHandler<ConsoleMenuEventArgs>? MenuEvent;

        public ConsoleOutputHandle OutputHandle { get; }

        internal ConsoleController(INativeCalls? api = null)
        {
            this.api = api ?? new NativeCalls();
            originalOutputHandle = this.api.GetOutputHandle();
            OutputHandle = this.api.CreateConsoleScreenBuffer();
            if (OutputHandle.IsInvalid)
                throw Exceptions.CouldNotCreateScreenBuffer();
            if (!this.api.SetActiveConsoleScreenBuffer(OutputHandle))
                throw Exceptions.CouldNotSetScreenBuffer();

            using var inputHandle = this.api.GetInputHandle();
            originalInputMode = this.api.GetConsoleMode(inputHandle);

            this.api.SetConsoleMode(inputHandle,
                                    ConsoleInputModes.EnableWindowInput |
                                    ConsoleInputModes.EnableMouseInput |
                                    ConsoleInputModes.EnableExtendedFlags);
            this.api.SetConsoleMode(OutputHandle, ConsoleOutputModes.None);

            new Thread(ListenerThread).Start();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0) return;
            Logger.Log(dbgctx, "Disposing.");
            stopEvent.Set();
            stopEvent.Dispose();
            using var inputHandle = api.GetInputHandle();
            api.SetConsoleMode(inputHandle, originalInputMode);
            api.SetActiveConsoleScreenBuffer(originalOutputHandle);
            originalOutputHandle.Dispose();
        }

        [SuppressMessage("Design", "CA1031", Justification = "Leave thread cleanly.")]
        void ListenerThread()
        {
            try
            {
                Logger.Log(dbgctx, "Starting thread.");
                using var inputHandle = api.GetInputHandle();
                IntPtr stdin = inputHandle.DangerousGetHandle();
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

                Logger.Log(dbgctx, "Stopping thread (stop event received).");
            }
            catch (ObjectDisposedException)
            {
                Logger.Log(dbgctx, "Stopping thread (stop event disposed).");
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
                using var inputHandle = api.GetInputHandle();
                var records = api.ReadConsoleInput(inputHandle);
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
                            OnSizeEvent();
                            break;
                        case InputEventType.Menu:
                            MenuEvent?.Invoke(this, new ConsoleMenuEventArgs(record.Event.MenuEvent));
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
        void OnSizeEvent()
        {
            var record = api.GetConsoleScreenBufferInfo(OutputHandle);
            Size bufferSize = new Size(record.BufferSize.X, record.BufferSize.Y);
            Rectangle windowArea = Rectangle.FromLTRB(
                left:  record.Window.Left,
                top: record.Window.Top,
                right: record.Window.Right,
                bottom: record.Window.Bottom);

            SizeEvent?.Invoke(this, new ConsoleSizeEventArgs(windowArea, bufferSize));
        }
    }
}
