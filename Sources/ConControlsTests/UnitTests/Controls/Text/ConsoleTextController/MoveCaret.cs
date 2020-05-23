/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Text.ConsoleTextController
{
    public partial class ConsoleTextControllerTests
    {
        [TestMethod]
        public void MoveCaret_Inconclusive()
        {
            Assert.Inconclusive();
        }
        //[TestMethod]
        //public void EndCaret_NotWrappedEmpty_PointEmpty()
        //{
        //    var sut = new ConControls.Controls.Text.ConsoleTextController();
        //    sut.EndCaret.Should().Be(Point.Empty);
        //}
        //[TestMethod]
        //public void EndCaret_NotWrappedNotEmptyLastLine_EndOfLastLine()
        //{
        //    const string text = "hello world!\ngood bye!";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(9, 1));
        //}
        //[TestMethod]
        //public void EndCaret_NotWrappedEmptyLastLine_NextLine()
        //{
        //    const string text = "hello world!\ngood bye!\n";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(0, 2));
        //}
        //[TestMethod]
        //public void EndCaret_NotWrappedTwoEmptyLastLines_NextLine()
        //{
        //    const string text = "hello world!\ngood bye!\n\r\n";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(0, 3));
        //}
        //[TestMethod]
        //public void EndCaret_WrappedEmpty_PointEmpty()
        //{
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Wrap = true
        //    };
        //    sut.EndCaret.Should().Be(Point.Empty);
        //}
        //[TestMethod]
        //public void EndCaret_WrappedNotEmptyOrFullLastLine_EndOfLine()
        //{
        //    const string text = "hello world!\ngood bye!";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Wrap = true,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(4, 4));
        //}
        //[TestMethod]
        //public void EndCaret_WrappedFullLastLine_NextLine()
        //{
        //    const string text = "01234\r\n01234";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Wrap = true,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(0, 3));
        //}
        //[TestMethod]
        //public void EndCaret_WrappedFullPrevAndEmptyLastLine_NextLine()
        //{
        //    const string text = "01234\r\n01234\n";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Wrap = true,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(0, 4));
        //}
        //[TestMethod]
        //public void EndCaret_WrappedFullPrevAndTwoEmptyLastLines_NextLine()
        //{
        //    const string text = "01234\r\n01234\r\n\n";
        //    var sut = new ConControls.Controls.Text.ConsoleTextController
        //    {
        //        Width = 5,
        //        Wrap = true,
        //        Text = text
        //    };
        //    sut.EndCaret.Should().Be(new Point(0, 5));
        //}
    }
}
