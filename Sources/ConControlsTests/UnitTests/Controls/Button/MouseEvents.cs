/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
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
        public void MouseEvents_EventArgsNull_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow);
            sut.Invoking(s => stubbedWindow.MouseEventEvent(stubbedWindow, null!)).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void MouseEvents_NoParent_Notthing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz()
            };
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(1, 1),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_Handled_Notthing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(1, 1),
                ButtonState = MouseButtonStates.LeftButtonPressed
            })) {Handled = true};
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_ClickedOutside_Notthing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(4, 4),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_RightClicked_Nothing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(1, 1),
                ButtonState = MouseButtonStates.RightButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            clicked.Should().BeFalse();
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_LeftClicked_EventRaised()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Size = (10, 3).Sz(),
                Parent = stubbedWindow
            };
            bool clicked = false;
            sut.Click += (sender, ea) => clicked = true;
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                MousePosition = new COORD(1, 1),
                ButtonState = MouseButtonStates.LeftButtonPressed
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            clicked.Should().BeTrue();
            e.Handled.Should().BeTrue();
        }
    }
}
