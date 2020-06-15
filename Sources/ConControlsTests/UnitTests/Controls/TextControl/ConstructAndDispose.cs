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
            using var window = new StubbedWindow();
            var textController = new StubbedConsoleTextController();

            using var sut = new StubbedTextControl(window, textController);
            sut.CursorVisible.Should().BeFalse();
            sut.CursorPosition.Should().Be(Point.Empty);
        }
    }
}
