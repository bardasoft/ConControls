/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void MouseEvents_MouseEventRaisedWithCorrectValues()
        {
            const ControlKeyStates controlKeys = ControlKeyStates.LEFT_ALT_PRESSED | ControlKeyStates.CAPSLOCK_ON;
            const int scroll = 123;
            const MouseButtonStates buttons = MouseButtonStates.LeftButtonPressed;
            Point position = (3, 7).Pt();
            const MouseEventFlags kind = MouseEventFlags.Wheeled;

            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();

            var args = new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                ButtonState = buttons,
                ControlKeys = controlKeys,
                EventFlags = kind,
                MousePosition = new COORD(position),
                Scroll = scroll
            });

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            bool raised = false;
            sut.MouseEvent += OnMouse;
            controller.MouseEventEvent(controller, args);
            raised.Should().BeTrue();
            raised = false;
            sut.MouseEvent-= OnMouse;
            controller.MouseEventEvent(controller, args);
            raised.Should().BeFalse();

            void OnMouse(object sender, MouseEventArgs e)
            {
                sender.Should().Be(sut);
                e.ControlKeys.Should().Be(controlKeys);
                e.Scroll.Should().Be(scroll);
                e.ButtonState.Should().Be(buttons);
                e.Position.Should().Be(position);
                e.Kind.Should().Be(kind);
                raised = true;
            }
        }
    }
}
