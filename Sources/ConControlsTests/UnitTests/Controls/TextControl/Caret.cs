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
        public void Caret_SetInside_ValidatedAndCursorUpdatedAndCursorVisible()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedTextController = new StubbedConsoleTextController();
            var size = new Size(10, 10);
            Point scroll = new Point(3, 4);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size),
                Scroll = scroll
            };

            sut.Caret.Should().Be(Point.Empty);
            bool caretValidated = false;
            Point caret = new Point(7, 8);
            stubbedTextController.ValidateCaretPoint = point =>
            {
                point.Should().Be(caret);
                caretValidated = true;
                return caret;
            };

            sut.Caret = caret;
            caretValidated.Should().BeTrue();
            sut.Caret.Should().Be(caret);
            sut.CursorVisible.Should().BeTrue();
            sut.CursorPosition.Should().Be(new Point(4, 4));
        }
        [TestMethod]
        public void Caret_SetBelow_ValidatedAndCursorInvisible()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedTextController = new StubbedConsoleTextController();
            var size = new Size(10, 10);
            Point scroll = new Point(3, 4);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size),
                Scroll = scroll
            };

            sut.Caret.Should().Be(Point.Empty);
            bool caretValidated = false;
            Point caret = new Point(15, 20);
            stubbedTextController.ValidateCaretPoint = point =>
            {
                point.Should().Be(caret);
                caretValidated = true;
                return caret;
            };

            sut.Caret = caret;
            caretValidated.Should().BeTrue();
            sut.Caret.Should().Be(caret);
            sut.CursorVisible.Should().BeFalse();
        }
        [TestMethod]
        public void Caret_SetAbive_ValidatedAndCursorInvisible()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedTextController = new StubbedConsoleTextController();
            var size = new Size(10, 10);
            Point scroll = new Point(3, 5);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size),
                Scroll = scroll
            };

            sut.Caret.Should().Be(Point.Empty);
            bool caretValidated = false;
            Point caret = new Point(3, 1);
            stubbedTextController.ValidateCaretPoint = point =>
            {
                point.Should().Be(caret);
                caretValidated = true;
                return caret;
            };

            sut.Caret = caret;
            caretValidated.Should().BeTrue();
            sut.Caret.Should().Be(caret);
            sut.CursorVisible.Should().BeFalse();
        }
    }
}
