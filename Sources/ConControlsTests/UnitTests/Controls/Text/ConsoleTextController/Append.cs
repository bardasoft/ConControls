/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Text.ConsoleTextController
{
    public partial class ConsoleTextControllerTests
    {
        [TestMethod]
        public void Append_Empty_Empty()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap
            };
            sut.Append(string.Empty);
            sut.MaxLineLength.Should().Be(0);
        }
        [TestMethod]
        public void Append_TestCase_001()
        {
            const string text = "01234\r\n56\n\r\n789\n012345678";
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap,
                Text = text
            };

            sut.BufferLineCount.Should().Be(7);
            sut.GetLineLength(0).Should().Be(5);
            sut.GetLineLength(1).Should().Be(0);
            sut.GetLineLength(2).Should().Be(2);
            sut.GetLineLength(3).Should().Be(0);
            sut.GetLineLength(4).Should().Be(3);
            sut.GetLineLength(5).Should().Be(5);
            sut.GetLineLength(6).Should().Be(4);

            sut.Append(text);
            sut.Text.Should().Be(text + text);
            sut.BufferLineCount.Should().Be(13);
            sut.MaxLineLength.Should().Be(5);
            sut.GetLineLength(0).Should().Be(5);
            sut.GetLineLength(1).Should().Be(0);
            sut.GetLineLength(2).Should().Be(2);
            sut.GetLineLength(3).Should().Be(0);
            sut.GetLineLength(4).Should().Be(3);
            sut.GetLineLength(5).Should().Be(5);
            sut.GetLineLength(6).Should().Be(5);
            sut.GetLineLength(7).Should().Be(4);
            sut.GetLineLength(8).Should().Be(2);
            sut.GetLineLength(9).Should().Be(0);
            sut.GetLineLength(10).Should().Be(3);
            sut.GetLineLength(11).Should().Be(5);
            sut.GetLineLength(12).Should().Be(4);

            sut.GetCharacters(new Rectangle(Point.Empty, new Size(5, 13)))
               .Should()
               .Equal(
                   '0', '1', '2', '3', '4',
                   '\0', '\0', '\0', '\0', '\0',
                   '5', '6', '\0', '\0', '\0',
                   '\0', '\0', '\0', '\0', '\0',
                   '7', '8', '9', '\0', '\0',
                   '0', '1', '2', '3', '4',
                   '5', '6', '7', '8', '0',
                   '1', '2', '3', '4', '\0',
                   '5', '6', '\0', '\0', '\0',
                   '\0', '\0', '\0', '\0', '\0',
                   '7', '8', '9', '\0', '\0',
                   '0', '1', '2', '3', '4', 
                   '5', '6', '7', '8', '\0'
                   );
        }
    }
}
