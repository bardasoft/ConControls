/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void ControlManagement_CursorUpdate_Updated()
        {
            bool cursorVisible = false;
            int cursorSize = 0;
            Point cursorPosition = Point.Empty;

            var api = new StubbedNativeCalls
            {
                SetCursorInfoConsoleOutputHandleBooleanInt32Point = (handle, visible, size, position) =>
                {
                    cursorVisible = visible;
                    cursorSize = size;
                    cursorPosition = position;
                }
            };
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();
            var textController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                EndCaretGet = () => (20, 20).Pt(),
                GetLineLengthInt32= l => 20
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            using var c = new ConControls.Controls.TextBlock(sut, textController)
                {Area = (5, 5, 10, 10).Rect(), Parent = sut, CursorSize = 12, CursorVisible = false, CursorPosition = (1, 2).Pt()};

            sut.FocusedControl = c;
            cursorVisible.Should().BeFalse();
            cursorSize.Should().Be(12);
            cursorPosition.Should().Be((6, 7).Pt());

            c.CursorPosition = (5, 6).Pt();
            cursorPosition.Should().Be((10, 11).Pt());

            c.CursorVisible = true;
            cursorVisible.Should().BeTrue();

            c.CursorSize = 23;
            cursorSize.Should().Be(23);
        }
        [TestMethod]
        public void ControlManagement_ControlAreaChanged_Redrawn()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();
            bool provided = false;
            graphicsProvider.ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, calls, arg3, arg4) =>
            {
                provided = true;
                return graphicsProvider.Graphics;
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            using var c = new Panel(sut) {Area = (5, 5, 10, 10).Rect(), Parent = sut};
            provided = false;
            c.Area = (4, 4, 6, 6).Rect();
            provided.Should().BeTrue();

            sut.Controls.Remove(c);
            provided = false;
            c.Area = (1, 2, 3, 4).Rect();
            provided.Should().BeFalse();
        }
    }
}
