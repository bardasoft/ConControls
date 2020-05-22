/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Text.ConsoleTextController
{
    public partial class ConsoleTextControllerTests
    {
        [TestMethod]
        public void Width_Applied()
        {
            const string text = "hello world!\ngood bye!";
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 4,
                Wrap = true,
                Text = text
            };

            sut.BufferLineCount.Should().Be(7);
            sut.Width.Should().Be(4);
            sut.Width = 4;
            sut.BufferLineCount.Should().Be(7);
            sut.Width.Should().Be(4);

            sut.Width = 7;
            sut.Width.Should().Be(7);
            sut.BufferLineCount.Should().Be(4);
            sut.Width = 7;
            sut.Width.Should().Be(7);
            sut.BufferLineCount.Should().Be(4);
        }
    }
}
