/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void OnWindowMouseEvent_EventArgsNull_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Invoking(s => stubbedWindow.MouseEventEvent(stubbedWindow, null!)).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void OnWindowMouseEvent_Handled_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.LeftButtonPressed
            })) {Handled = true};
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_Disabled_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true,
                Enabled = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
                {
                    MousePosition = new COORD(12, 12),
                    ButtonState = MouseButtonStates.LeftButtonPressed
                }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_Invisible_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true,
                Visible = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_CantFocus_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_WrongButtton_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.RightButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_Outside_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(4, 4),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnWindowMouseEvent_LeftClickedInside_Focused()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow,
                Focusable = true
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Focused.Should().BeTrue();
            e.Handled.Should().BeTrue();
        }
    }
}
