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
        public void MoveCaretPageUp_UnwrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };
            sut.MoveCaretPageUp(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageUp(new Point(1, 1), 5).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaretPageUp_WrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };
            sut.MoveCaretPageUp(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageUp(new Point(1, 1), 5).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaretPageUp_UnwrappedNonEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Line1\nLine2\nLine3\nLongLine4",
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };
            sut.MoveCaretPageUp(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageUp(new Point(1, 1), 5).Should().Be(new Point(1,0));
            sut.MoveCaretPageUp(new Point(3, 3), 2).Should().Be(new Point(3, 1));
            sut.MoveCaretPageUp(new Point(6, 3), 2).Should().Be(new Point(5, 1));
        }
        [TestMethod]
        public void MoveCaretPageUp_WrappedNonEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Line1Line2L3\nLine4",
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };
            sut.MoveCaretPageUp(Point.Empty, 5).Should().Be(Point.Empty);
            sut.MoveCaretPageUp(new Point(1, 1), 5).Should().Be(new Point(1,0));
            sut.MoveCaretPageUp(new Point(3, 3), 1).Should().Be(new Point(2, 2));
        }
    }
}
