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
        public void ValidateCaret_Empty_PointEmpty()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap,
                Text = string.Empty
            };

            sut.BufferLineCount.Should().Be(0);

            Point valid = Point.Empty;
            Point tooLeft = new Point(-1, 0);
            Point tooUp = new Point(0, -1);
            Point tooRight = new Point(5, 0);
            Point tooDown = new Point(0, 6);

            sut.ValidateCaret(valid).Should().Be(valid);
            sut.ValidateCaret(tooLeft).Should().Be(valid);
            sut.ValidateCaret(tooUp).Should().Be(valid);
            sut.ValidateCaret(tooRight).Should().Be(valid);
            sut.ValidateCaret(tooDown).Should().Be(valid);
        }
        [TestMethod]
        public void ValidateCaret_AdjustedCaretProperly()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap,
                Text = "0123456789\n0123456789"
            };

            sut.BufferLineCount.Should().Be(6);

            Point valid = new Point(1, 1);
            Point tooLeft = new Point(-1, 1);
            Point tooUp = new Point(1, -1);
            Point tooRight = new Point(5, 1);
            Point tooDown = new Point(1, 6);

            Point left = new Point(0, 1);
            Point up = new Point(1, 0);
            Point right = new Point(4, 1);
            Point down = new Point(0, 5);

            sut.ValidateCaret(valid).Should().Be(valid);
            sut.ValidateCaret(tooLeft).Should().Be(left);
            sut.ValidateCaret(tooUp).Should().Be(up);
            sut.ValidateCaret(tooRight).Should().Be(right);
            sut.ValidateCaret(tooDown).Should().Be(down);
        }
        [TestMethod]
        public void ValidateCaret_NotWrapped_LowerEnd()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.NoWrap,
                Text = "0123456789\n0123456789"
            };

            sut.BufferLineCount.Should().Be(2);

            var endPoint = new Point(10, 1);
            var tooRight = new Point(11, 1);
            var tooLow = new Point(0, 2);
            
            sut.ValidateCaret(endPoint).Should().Be(endPoint);
            sut.ValidateCaret(tooRight).Should().Be(endPoint);
            sut.ValidateCaret(tooLow).Should().Be(new Point(0, 1));
        }
        [TestMethod]
        public void ValidateCaret_Wrapped_LowerEnd()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap,
                Text = "0123456789\n0123456789"
            };

            sut.BufferLineCount.Should().Be(6);

            var endPoint = new Point(0, 5);
            var tooRight = new Point(1, 5);
            var tooLow = new Point(0, 6);

            sut.ValidateCaret(endPoint).Should().Be(endPoint);
            sut.ValidateCaret(tooRight).Should().Be(endPoint);
            sut.ValidateCaret(tooLow).Should().Be(new Point(0, 5));
        }
    }
}
