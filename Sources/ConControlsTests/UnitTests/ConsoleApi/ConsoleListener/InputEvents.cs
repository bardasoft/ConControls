/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.Threading;
using System.Threading.Tasks;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleListener
{
    public partial class ConsoleListenerTests
    {
        [TestMethod]
        // ReSharper disable once FunctionComplexityOverflow
        public async Task InputEvents_AllKindsOfEvents_EventsFired()
        {
            var keyRecord = new INPUT_RECORD
            {
                EventType = InputEventType.Key,
                Event = new INPUT_RECORD.EVENTUNION
                {
                    KeyEvent = new KEY_EVENT_RECORD
                    {
                        ControlKeys = ControlKeyStates.LEFT_ALT_PRESSED,
                        KeyDown = 12,
                        RepeatCount = 23,
                        UnicodeChar = 'x',
                        VirtualKeyCode = VirtualKey.Accept,
                        VirtualScanCode = 42
                    }
                }
            };
            var mouseRecord = new INPUT_RECORD
            {
                EventType = InputEventType.Mouse,
                Event = new INPUT_RECORD.EVENTUNION
                {
                    MouseEvent = new MOUSE_EVENT_RECORD
                    {
                        ControlKeys = ControlKeyStates.LEFT_ALT_PRESSED,
                        EventFlags = MouseEventFlags.DoubleClick,
                        ButtonState = MouseButtonStates.FourthButtonPressed,
                        MousePosition = new COORD(23,42),
                        Scroll = 17
                    }
                }
            };
            var sizeRecord = new INPUT_RECORD
            {
                EventType = InputEventType.WindowBufferSize,
                Event = new INPUT_RECORD.EVENTUNION
                {
                    SizeEvent = new WINDOW_BUFFER_SIZE_RECORD
                    {
                        Size = new COORD(23, 42)
                    }
                }
            };
            var menuRecord = new INPUT_RECORD
            {
                EventType = InputEventType.Menu,
                Event = new INPUT_RECORD.EVENTUNION
                {
                    MenuEvent = new MENU_EVENT_RECORD
                    {
                        CommandId = 123
                    }
                }
            };
            var focusRecord = new INPUT_RECORD
            {
                EventType = InputEventType.Focus,
                Event = new INPUT_RECORD.EVENTUNION
                {
                    FocusEvent = new FOCUS_EVENT_RECORD
                    {
                        SetFocus = 123
                    }
                }
            };

            var records = new[] {keyRecord, 
                mouseRecord, 
                sizeRecord,
                menuRecord,
                focusRecord
            };
            var keyTcs = new TaskCompletionSource<int>();
            var mouseTcs = new TaskCompletionSource<int>();
            var sizeTcs = new TaskCompletionSource<int>();
            var menuTcs = new TaskCompletionSource<int>();
            var focusTcs = new TaskCompletionSource<int>();

            using var stdinEvent = new AutoResetEvent(false);
            ConsoleInputHandle consoleInputHandle = new ConsoleInputHandle(stdinEvent.SafeWaitHandle.DangerousGetHandle());

            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => consoleInputHandle,
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                ReadConsoleInputConsoleInputHandleInt32 = (handle, size) =>
                {
                    handle.Should().Be(consoleInputHandle);
                    return records;
                }
            };
            using var sut = new ConControls.ConsoleApi.ConsoleListener(Console.OutputEncoding, api);
            sut.KeyEvent += (sender, e) =>
            {
                e.KeyDown.Should().Be(keyRecord.Event.KeyEvent.KeyDown != 0);
                e.UnicodeChar.Should().Be(keyRecord.Event.KeyEvent.UnicodeChar);
                e.RepeatCount.Should().Be(keyRecord.Event.KeyEvent.RepeatCount);
                e.VirtualKeyCode.Should().Be(keyRecord.Event.KeyEvent.VirtualKeyCode);
                e.VirtualScanCode.Should().Be(keyRecord.Event.KeyEvent.VirtualScanCode);
                e.ControlKeys.Should().Be(keyRecord.Event.KeyEvent.ControlKeys);
                keyTcs.SetResult(0);
            };
            sut.MouseEvent += (sender, e) =>
            {
                e.ButtonState.Should().Be(mouseRecord.Event.MouseEvent.ButtonState);
                e.EventFlags.Should().Be(mouseRecord.Event.MouseEvent.EventFlags);
                e.MousePosition.X.Should().Be(mouseRecord.Event.MouseEvent.MousePosition.X);
                e.MousePosition.Y.Should().Be(mouseRecord.Event.MouseEvent.MousePosition.Y);
                e.Scroll.Should().Be(mouseRecord.Event.MouseEvent.Scroll);
                e.ControlKeys.Should().Be(mouseRecord.Event.MouseEvent.ControlKeys);
                mouseTcs.SetResult(0);
            };
            sut.SizeEvent += (sender, e) =>
            {
                e.Size.Width.Should().Be(sizeRecord.Event.SizeEvent.Size.X);
                e.Size.Height.Should().Be(sizeRecord.Event.SizeEvent.Size.Y);
                sizeTcs.SetResult(0);
            };
            sut.MenuEvent += (sender, e) =>
            {
                e.CommandId.Should().Be(menuRecord.Event.MenuEvent.CommandId);
                menuTcs.SetResult(0);
            };
            sut.FocusEvent += (sender, e) =>
            {
                e.SetFocus.Should().Be(focusRecord.Event.FocusEvent.SetFocus != 0);
                focusTcs.SetResult(0);
            };


            var allTasks = Task.WhenAll(new Task[]
            {
                keyTcs.Task,
                mouseTcs.Task,
                sizeTcs.Task,
                menuTcs.Task,
                focusTcs.Task
            });
            stdinEvent.Set();
            //await allTasks;
            (await Task.WhenAny(allTasks, Task.Delay(2000)))
                .Should()
                .Be(allTasks, "events should be processed in less than 2 seconds!");

        }
        [TestMethod]
        // ReSharper disable once FunctionComplexityOverflow
        public async Task InputEvents_InvalidRecordType_Logged()
        {
            var record = new INPUT_RECORD
            {
                EventType = (InputEventType)0xFFFF
            };
            var records = new[] {record};
            var tcs = new TaskCompletionSource<int>();

            using var stdinEvent = new AutoResetEvent(false);
            ConsoleInputHandle consoleInputHandle = new ConsoleInputHandle(stdinEvent.SafeWaitHandle.DangerousGetHandle());

            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => consoleInputHandle,
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                ReadConsoleInputConsoleInputHandleInt32 = (handle, size) =>
                {
                    handle.Should().Be(consoleInputHandle);
                    return records;
                }
            };
            using var sut = new ConControls.ConsoleApi.ConsoleListener(Console.OutputEncoding, api);
            using var logger = new TestLogger(CheckInputLogForRecord);
            stdinEvent.Set();
            (await Task.WhenAny(tcs.Task, Task.Delay(2000)))
                .Should()
                .Be(tcs.Task, "event should be processed in less than 2 seconds!");

            void CheckInputLogForRecord(string msg)
            {
                if (msg.Contains("Unkown input record type "))
                    tcs.SetResult(0);
            }
        }
        [TestMethod]
        // ReSharper disable once FunctionComplexityOverflow
        public async Task InputEvents_Exception_Logged()
        {
            const string message = "--message--";
            var tcs = new TaskCompletionSource<int>();
            int exceptionCount = 0;

            using var stdinEvent = new AutoResetEvent(false);
            ConsoleInputHandle consoleInputHandle = new ConsoleInputHandle(stdinEvent.SafeWaitHandle.DangerousGetHandle());

            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => consoleInputHandle,
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                ReadConsoleInputConsoleInputHandleInt32 = (handle, size) => throw new Exception(message)
            };
            using var sut = new ConControls.ConsoleApi.ConsoleListener(Console.OutputEncoding, api);
            using var logger = new TestLogger(CheckInputLogForException);
            stdinEvent.Set();
            (await Task.WhenAny(tcs.Task, Task.Delay(2000)))
                .Should()
                .Be(tcs.Task, "event should be processed in less than 2 seconds!");

            void CheckInputLogForException(string msg)
            {
                if (!msg.Contains(message)) return;
                if (Interlocked.Increment(ref exceptionCount) == 2)
                    tcs.SetResult(0);
            }
        }
    }
}