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
        public void CursorVisible_Dependency()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedTextController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            var size = new Size(10, 10);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size)
            };

            sut.CursorVisible.Should().BeFalse();
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();

            sut.Scroll = new Point(0, 5);
            sut.CursorVisible.Should().BeFalse();
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeFalse();
            sut.Caret = new Point(1, 7);
            sut.CursorVisible.Should().BeTrue();
        }

    }
}
