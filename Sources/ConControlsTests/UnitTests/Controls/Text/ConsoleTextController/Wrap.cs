/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls.Text;
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

            sut.WrapMode.Should().Be(WrapMode.NoWrap);
            sut.BufferLineCount.Should().Be(2);
            sut.WrapMode = WrapMode.NoWrap;
            sut.WrapMode.Should().Be(WrapMode.NoWrap);
            sut.BufferLineCount.Should().Be(2);

            sut.WrapMode = WrapMode.SimpleWrap;
            sut.WrapMode.Should().Be(WrapMode.SimpleWrap);
            sut.BufferLineCount.Should().Be(7);
            sut.WrapMode = WrapMode.SimpleWrap;
            sut.WrapMode.Should().Be(WrapMode.SimpleWrap);
            sut.BufferLineCount.Should().Be(7);
        }
    }
}
