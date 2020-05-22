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

namespace ConControlsTests.UnitTests.Controls.Text.ConsoleTextController
{
    public partial class ConsoleTextControllerTests
    {
        [TestMethod]
        public void ValidateCaret_AdjustedCaretProperly()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                Wrap = true,
                Text = "0123456789\n0123456789"
            };

            sut.BufferLineCount.Should().Be(6);

            Point valid = new Point(1, 1);
            Point tooLeft = new Point(-1, 1);
            Point tooUp = new Point(1, -1);
            Point tooRight = new Point(5, 1);
            Point tooDown = new Point(1, 7);

            Point left = new Point(0, 1);
            Point up = new Point(1, 0);
            Point right = new Point(4, 1);
            Point down = new Point(0, 6);

            sut.ValidateCaret(valid).Should().Be(valid);
            sut.ValidateCaret(tooLeft).Should().Be(left);
            sut.ValidateCaret(tooUp).Should().Be(up);
            sut.ValidateCaret(tooRight).Should().Be(right);
            sut.ValidateCaret(tooDown).Should().Be(down);

        }
    }
}
