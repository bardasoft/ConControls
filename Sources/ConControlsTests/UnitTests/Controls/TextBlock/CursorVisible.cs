/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using System.Linq;
using ConControls.ConsoleApi;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextBlock
{
    public partial class TextBlockTests
    {
        [TestMethod]
        public void CursorVisible_Get_True()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                Size = new Size(10, 10)
            };
            sut.CursorVisible.Should().BeTrue();
        }
        [TestMethod]
        public void CursorVisible_CaretOutside_False()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                Size = new Size(10, 10)
            };
            sut.CursorVisible.Should().BeTrue();
            sut.Caret = (11, 11).Pt();
            sut.CaretVisible.Should().BeFalse();
            sut.CursorVisible.Should().BeFalse();
        }
        [TestMethod]
        public void CursorVisible_Set_EventOnlyOnChange()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                Size = (10,10).Sz()
            };
            int raised = 0;
            sut.CursorVisibleChanged += (sender, e) => raised++;
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(1); // base value is false
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(1);
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();
            raised.Should().Be(2);
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();
            raised.Should().Be(2);
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(3);
        }
        [TestMethod]
        public void CursorVisible_IntegrationTest_RespectsCaretVisibility()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            string text = string.Join(Environment.NewLine, Enumerable.Repeat("12345678901234567890", 40));
            using var window = new ConControls.Controls.ConsoleWindow(api, controller, new StubbedGraphicsProvider());
            using var sut = new ConControls.Controls.TextBlock(window)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = window,
                Text = text,
                Caret = (5, 0).Pt(),
                CanFocus = true,
                Focused = true
            };

            sut.Caret.Should().Be((5, 0).Pt());
            sut.CursorVisible.Should().BeTrue();
            sut.CursorPosition.Should().Be((5, 0).Pt());
            var e = new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = -120
            });

            bool setCorrectly = false;
            api.SetCursorInfoConsoleOutputHandleBooleanInt32Point = (handle, visible, size, location) => setCorrectly = !visible;
            controller.MouseEventEvent(controller, e);
            setCorrectly.Should().BeTrue();
            sut.Scroll.Should().Be((0, 1).Pt());
            sut.CursorVisible.Should().BeFalse();
            sut.Caret.Should().Be((5, 0).Pt());
        }
    }
}
