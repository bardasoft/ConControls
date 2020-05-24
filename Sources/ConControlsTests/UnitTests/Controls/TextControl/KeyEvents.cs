/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void KeyEvents_EVentArgsNull_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };

            sut.Invoking(s => stubbedWindow.KeyEventEvent(stubbedWindow, null!)).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void KeyEvents_Handled_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
                {
                    KeyDown = 1,
                    VirtualKeyCode = VirtualKey.Up
                }))
                { Handled = true };
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((10, 10).Pt());
            sut.Scroll.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void KeyEvents_NotFocused_Nothing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            sut.Focused.Should().BeFalse();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
                {
                    KeyDown = 1,
                    VirtualKeyCode = VirtualKey.Up
                }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((10, 10).Pt());
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void KeyEvents_Disabled_Nothing()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt(),
                Enabled = false
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Up
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((10, 10).Pt());
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void KeyEvents_Invisible_Nothing()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt(),
                Visible = false
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Up
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((10, 10).Pt());
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void KeyEvents_Released_Nothing()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 0,
                VirtualKeyCode = VirtualKey.Up
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((10, 10).Pt());
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void KeyEvents_Up_MovedUpAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretUpPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Up
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Down_MovedDownAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretDownPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Down
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Left_MovedLeftAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretLeftPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Left
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Right_MovedRightAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretRightPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Right
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Home_MovedToBeginOfLineAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretToBeginOfLinePoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Home
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_LeftCtrlHome_MovedHomeAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretHomePoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Home,
                ControlKeys = ControlKeyStates.LEFT_CTRL_PRESSED
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_RightCtrlHome_MovedHomeAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretHomePoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Home,
                ControlKeys = ControlKeyStates.RIGHT_CTRL_PRESSED
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_End_MovedToEndOfLineAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretToEndOfLInePoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.End
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_LeftCtrlEnd_MovedToEndAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretEndPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.End,
                ControlKeys = ControlKeyStates.LEFT_CTRL_PRESSED
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_RightCtrlEnd_MovedToEndAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretEndPoint = p =>
                {
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.End,
                ControlKeys = ControlKeyStates.RIGHT_CTRL_PRESSED
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Prior_MovedPageUpAndScrollled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretPageUpPointInt32 = (p, l) =>
                {
                    l.Should().Be(10);
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Prior
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void KeyEvents_Next_MovedPageDownAndScrolled()
        {
            StubbedTextControl? focused = null;

            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused
            };
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                EndCaretGet = () => (0, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MoveCaretPageDownPointInt32 = (p, l) =>
                {
                    l.Should().Be(10);
                    p.Should().Be((10, 10).Pt());
                    return (13, 14).Pt();
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Caret = (10, 10).Pt()
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                VirtualKeyCode = VirtualKey.Next
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            sut.Caret.Should().Be((13, 14).Pt());
            sut.Scroll.Should().Be((4, 5).Pt());
            e.Handled.Should().BeTrue();
        }
    }
}
