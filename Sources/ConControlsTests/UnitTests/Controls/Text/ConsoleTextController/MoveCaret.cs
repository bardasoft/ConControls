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
        public void MoveCaret_UnwrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretToBeginOfLine(new Point(1, 0)).Should().Be(Point.Empty);
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretRight(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretDown(Point.Empty).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaret_WrappedEmpty_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretToBeginOfLine(new Point(1, 0)).Should().Be(Point.Empty);
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretRight(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretDown(Point.Empty).Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MoveCaret_UnwrappedNonEmptyLastLine_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hello\nWor",
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(new Point(3, 1));
            sut.MoveCaretToBeginOfLine(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(new Point(5, 0));
            sut.MoveCaretToEndOfLIne(new Point(1, 1)).Should().Be(new Point(3, 1));
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretLeft(new Point(0, 1)).Should().Be(new Point(5, 0));
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(new Point(6, 1)).Should().Be(new Point(5, 0));
            sut.MoveCaretRight(Point.Empty).Should().Be(new Point(1, 0));
            sut.MoveCaretRight(new Point(5, 0)).Should().Be(new Point(0, 1));
            sut.MoveCaretRight(new Point(3, 1)).Should().Be(new Point(3, 1));
            sut.MoveCaretDown(Point.Empty).Should().Be(new Point(0, 1));
            sut.MoveCaretDown(new Point(5, 0)).Should().Be(new Point(3, 1));
        }
        [TestMethod]
        public void MoveCaret_UnwrappedEmptyLastLine_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hello\nWorld!\n",
                WrapMode = WrapMode.NoWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(new Point(0, 2));
            sut.MoveCaretToBeginOfLine(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(new Point(5, 0));
            sut.MoveCaretToEndOfLIne(new Point(1, 2)).Should().Be(new Point(0, 2));
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretLeft(new Point(0, 2)).Should().Be(new Point(6, 1));
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(new Point(6, 1)).Should().Be(new Point(5, 0));
            sut.MoveCaretRight(Point.Empty).Should().Be(new Point(1, 0));
            sut.MoveCaretRight(new Point(5, 0)).Should().Be(new Point(0, 1));
            sut.MoveCaretRight(new Point(0, 2)).Should().Be(new Point(0, 2));
            sut.MoveCaretDown(Point.Empty).Should().Be(new Point(0, 1));
            sut.MoveCaretDown(new Point(3, 1)).Should().Be(new Point(0, 2));
        }
        [TestMethod]
        public void MoveCaret_WrappedNonEmptyLastLine_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hi\nWor",
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(new Point(3, 1));
            sut.MoveCaretToBeginOfLine(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(new Point(2, 0));
            sut.MoveCaretToEndOfLIne(new Point(1, 1)).Should().Be(new Point(3, 1));
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretLeft(new Point(0, 2)).Should().Be(new Point(3, 1));
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(new Point(4, 1)).Should().Be(new Point(2, 0));
            sut.MoveCaretRight(Point.Empty).Should().Be(new Point(1, 0));
            sut.MoveCaretRight(new Point(2, 0)).Should().Be(new Point(0, 1));
            sut.MoveCaretDown(Point.Empty).Should().Be(new Point(0, 1));
            sut.MoveCaretDown(new Point(3, 1)).Should().Be(new Point(3, 1));
        }
        [TestMethod]
        public void MoveCaret_WrappedEmptyLastLine_CorrectResults()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hi\nWorld",
                WrapMode = WrapMode.SimpleWrap,
                Width = 5
            };

            sut.MoveCaretHome(new Point(1, 1)).Should().Be(Point.Empty);
            sut.MoveCaretEnd(new Point(1, 1)).Should().Be(new Point(0, 2));
            sut.MoveCaretToBeginOfLine(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretToEndOfLIne(Point.Empty).Should().Be(new Point(2, 0));
            sut.MoveCaretToEndOfLIne(new Point(1, 1)).Should().Be(new Point(4, 1));
            sut.MoveCaretToEndOfLIne(new Point(1, 2)).Should().Be(new Point(0, 2));
            sut.MoveCaretLeft(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretLeft(new Point(1, 1)).Should().Be(new Point(0, 1));
            sut.MoveCaretLeft(new Point(0, 2)).Should().Be(new Point(4, 1));
            sut.MoveCaretUp(Point.Empty).Should().Be(Point.Empty);
            sut.MoveCaretUp(new Point(4, 1)).Should().Be(new Point(2, 0));
            sut.MoveCaretRight(Point.Empty).Should().Be(new Point(1, 0));
            sut.MoveCaretRight(new Point(2, 0)).Should().Be(new Point(0, 1));
            sut.MoveCaretRight(new Point(0, 2)).Should().Be(new Point(0, 2));
            sut.MoveCaretDown(Point.Empty).Should().Be(new Point(0, 1));
            sut.MoveCaretDown(new Point(3, 1)).Should().Be(new Point(0, 2));
        }
    }
}
