/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void ConstructAndDispsoed_EventsWired_EventsUnwired()
        {
            const int cursorSize = 42;
            using var window = new StubbedWindow
            {
                CursorSizeGet = () => cursorSize
            };
            var textController = new StubbedConsoleTextController();

            using var sut = new StubbedTextControl(window, textController);
            sut.CursorSize.Should().Be(cursorSize);
            sut.CursorVisible.Should().BeTrue();
            sut.CursorPosition.Should().Be(Point.Empty);

            textController.BufferChangedEvent.Should().NotBeNull();
            textController.CaretChangedEvent.Should().NotBeNull();
            
            sut.Dispose();

            textController.BufferChangedEvent.Should().BeNull();
            textController.CaretChangedEvent.Should().BeNull();
        }
    }
}
