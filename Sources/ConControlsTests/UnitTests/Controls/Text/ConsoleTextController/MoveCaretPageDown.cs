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
        public void MoveCaretPageDown_UnwrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };
            sut.MoveCaretPageDown(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageDown(new Point(1, 1), 5).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaretPageDown_WrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };
            sut.MoveCaretPageDown(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageDown(new Point(1, 1), 5).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaretPageDown_UnwrappedNonEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Line1\nLine2\nLongLine3\nLine4",
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };
            sut.MoveCaretPageDown(Point.Empty, 5).Should().Be(new Point(0, 3));
            sut.MoveCaretPageDown(new Point(1, 1), 5).Should().Be(new Point(1,3));
            sut.MoveCaretPageDown(new Point(3, 3), 2).Should().Be(new Point(3, 3));
            sut.MoveCaretPageDown(new Point(7, 2), 2).Should().Be(new Point(5, 3));
        }
        [TestMethod]
        public void MoveCaretPageDown_WrappedNonEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Line1Line2L3\nLine4",
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };
            sut.MoveCaretPageDown(Point.Empty, 5).Should().Be(new Point(0, 4));
            sut.MoveCaretPageDown(new Point(1, 1), 5).Should().Be(new Point(0,4));
            sut.MoveCaretPageDown(new Point(3, 0), 2).Should().Be(new Point(2, 2));
        }
    }
}
