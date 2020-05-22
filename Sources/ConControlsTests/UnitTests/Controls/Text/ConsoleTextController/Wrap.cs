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
        public void Wrap_Applied()
        {
            const string text = "hello world!\ngood bye!";
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 4,
                Text = text
            };

            sut.Wrap.Should().BeFalse();
            sut.BufferLineCount.Should().Be(2);
            sut.Wrap = false;
            sut.Wrap.Should().BeFalse();
            sut.BufferLineCount.Should().Be(2);

            sut.Wrap = true;
            sut.Wrap.Should().BeTrue();
            sut.BufferLineCount.Should().Be(7);
            sut.Wrap = true;
            sut.Wrap.Should().BeTrue();
            sut.BufferLineCount.Should().Be(7);
        }
    }
}
