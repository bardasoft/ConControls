/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

#nullable enable

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void OnKeyEvent_Handled_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Return
            })) {Handled = true};
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_NotFocused_Notthing()
        {
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => null
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            sut.Focused.Should().BeFalse();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
                {
                    KeyDown = 1,
                    ControlKeys = ControlKeyStates.NUMLOCK_ON,
                    VirtualKeyCode = VirtualKey.Return
                }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_Disabled_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow,
                Enabled = false
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Return
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_Invisible_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow,
                Visible = false
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Return
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_KeyUp_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 0,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Return
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_ControlKeys_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SHIFT_PRESSED,
                VirtualKeyCode = VirtualKey.Return
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_WrongKey_Notthing()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.X
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnKeyEvent_ReturnKey_Clicked()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Return
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeTrue();
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void OnKeyEvent_SpaceKey_Clicked()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            using var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            focused = sut;
            sut.Focused.Should().BeTrue();
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new KeyEventArgs(new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.Space
            }));
            stubbedWindow.KeyEventEvent(stubbedWindow, e);
            clicked.Should().BeTrue();
            e.Handled.Should().BeTrue();
        }
    }
}
