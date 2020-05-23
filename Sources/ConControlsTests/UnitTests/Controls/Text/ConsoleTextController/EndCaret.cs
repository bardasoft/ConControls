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
        public void EndCaret_UnwrappedEmpty_Empty()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                Wrap = false
            };
            sut.EndCaret.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void EndCaret_WrappedEmpty_Empty()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                Wrap = true
            };
            sut.EndCaret.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void EndCaret_UnwrappedNonEmptyLastLine_CorrectResult()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hello\nWorld",
                Width = 5,
                Wrap = false
            };
            sut.EndCaret.Should().Be(new Point(5, 1));
        }
        [TestMethod]
        public void EndCaret_UnwrappedEmptyLastLine_CorrectResult()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hello\nWorld\n",
                Width = 5,
                Wrap = false
            };
            sut.EndCaret.Should().Be(new Point(0, 2));
        }
        [TestMethod]
        public void EndCaret_WrappedNonEmptyLastLine_CorrectResult()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "Hello\nWor",
                Width = 5,
                Wrap = true
            };
            sut.EndCaret.Should().Be(new Point(3, 2));
        }
        [TestMethod]
        public void EndCaret_WrappedEmptyLastLine_CorrectResult()
        {
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Text = "HelloWorld\n",
                Width = 5,
                Wrap = true
            };
            sut.EndCaret.Should().Be(new Point(0, 3));
        }
    }
}
