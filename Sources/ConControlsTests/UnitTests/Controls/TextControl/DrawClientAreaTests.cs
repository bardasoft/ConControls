/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void DrawClientArea_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController();
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController);
            sut.Invoking(s => s.CallDrawClientArea()).Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void DrawClientArea_DrawingInhibited_NotDrawn()
        {
            const ConsoleColor foreColor = ConsoleColor.Cyan;
            const ConsoleColor backColor = ConsoleColor.Blue;
            bool drawn = false;
            using var stubbedWindow = new StubbedWindow
            {
                BackgroundColorGet = () => backColor,
                ForegroundColorGet = () => foreColor,
                DrawingInhibitedGet = () => true
            };
            stubbedWindow.Graphics.CopyCharactersConsoleColorConsoleColorPointCharArraySize = (bg, fg, topLeft, characters, charsSize) => drawn = true;
            var stubbedController = new StubbedConsoleTextController
            {
                GetCharactersRectangle = rectangle => new char[10]
            };
            var size = new Size(23, 42);
            var scroll = new Point(2, 3);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = new Rectangle(Point.Empty, size),
                BorderStyle = BorderStyle.SingleLined,
                Scroll = scroll,
                Parent = stubbedWindow
            };
           
            sut.CallDrawClientArea(stubbedWindow.Graphics);
            drawn.Should().BeFalse();
        }
        [TestMethod]
        public void DrawClientArea_DrawingAllowed_DrawnCorrectly()
        {
            const ConsoleColor foreColor = ConsoleColor.Cyan;
            const ConsoleColor backColor = ConsoleColor.Blue;
            using var stubbedWindow = new StubbedWindow
            {
                BackgroundColorGet = () => backColor,
                ForegroundColorGet = () => foreColor
            };
            var stubbedController = new StubbedConsoleTextController();
            var size = new Size(23, 42);
            var scroll = new Point(2, 3);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = new Rectangle(Point.Empty, size),
                BorderStyle = BorderStyle.SingleLined,
                Scroll = scroll,
                Parent = stubbedWindow
            };

            char[] expectedChars = new char[10];
            stubbedController.GetCharactersRectangle = rectangle =>
            {
                rectangle.Should().Be(new Rectangle(scroll.X, scroll.Y, size.Width - 2, size.Height - 2));
                return expectedChars;
            };
            bool drawn = false;
            stubbedWindow.Graphics.CopyCharactersConsoleColorConsoleColorPointCharArraySize = (bg, fg, topLeft, characters, charsSize) =>
            {
                bg.Should().Be(backColor);
                fg.Should().Be(foreColor);
                topLeft.Should().Be(new Point(1, 1));
                characters.Should().BeSameAs(expectedChars);
                charsSize.Should().Be(new Size(size.Width - 2, size.Height - 2));
                drawn = true;
            };

            sut.Draw();
            drawn.Should().BeTrue();
        }
    }
}
