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

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void Scroll_Scrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedTextController = new StubbedConsoleTextController();
            var size = new Size(10, 10);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size)
            };

            sut.Scroll.Should().Be(Point.Empty);

            Point scroll = new Point(3, 4);
            bool charactersRequestedCorrectly = false;
            stubbedTextController.GetCharactersRectangle = rectangle =>
            {
                rectangle.Should().Be(new Rectangle(scroll, size));
                charactersRequestedCorrectly = true;
                return new char[100];
            };

            sut.Scroll = scroll;
            charactersRequestedCorrectly.Should().BeTrue();
            sut.Scroll.Should().Be(scroll);
        }
    }
}
