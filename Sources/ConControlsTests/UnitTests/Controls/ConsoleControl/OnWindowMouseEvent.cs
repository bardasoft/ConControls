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

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
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

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
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

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
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

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_MoveInsideAndOutside_EnterMoveAndLeave()
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
            var enterArgs = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Moved,
                MousePosition = new COORD(6, 6),
                ButtonState = MouseButtonStates.None
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, enterArgs);

            sut.Focused.Should().BeFalse();
            enterArgs.Handled.Should().BeTrue();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);

            var leaveArgs = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Moved,
                MousePosition = new COORD(4, 4),
                ButtonState = MouseButtonStates.None
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, leaveArgs);

            sut.Focused.Should().BeFalse();
            leaveArgs.Handled.Should().BeTrue();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_RightClickedInside_ClickCalledButNotFocused()
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
                EventFlags = MouseEventFlags.None,
                MousePosition = new COORD(12, 12),
                ButtonState = MouseButtonStates.RightButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);

            sut.Focused.Should().BeFalse();
            e.Handled.Should().BeTrue($"{nameof(StubbedConsoleControl)} should always set handled flag");

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_LeftClickedInsideButCantFocus_ClickCalledButNotFocused()
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
            e.Handled.Should().BeTrue($"{nameof(StubbedConsoleControl)} should always set handled flag");

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_LeftClickedInside_FocusedAndOnMouseClick()
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

            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_MouseMoved_OnMouseMovedCalled()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Moved,
                MousePosition = new COORD(12, 12)
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, e);

            e.Handled.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_MouseDoubleClicked_OnMouseDoubleClickCalled()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.DoubleClick,
                MousePosition = new COORD(12, 12)
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, e);

            e.Handled.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowMouseEvent_MouseWheeledVertically_OnMouseScrollCalled()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(12, 12)
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, e);

            e.Handled.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(1);
        }
        [TestMethod]
        public void OnWindowMouseEvent_MouseWheeledHorizontally_OnMouseScrollCalled()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (5, 5, 10, 10).Rect(),
                BorderStyle = BorderStyle.DoubleLined,
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(12, 12)
            }));

            stubbedWindow.MouseEventEvent(stubbedWindow, e);

            e.Handled.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEnter).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseLeave).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseMove).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseDoubleClick).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseScroll).Should().Be(1);
        }
    }
}
