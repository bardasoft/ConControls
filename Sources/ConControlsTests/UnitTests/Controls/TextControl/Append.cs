/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void Append_Null_ArgumentNullException()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedTextControl(stubbedWindow, new StubbedConsoleTextController());
            sut.Invoking(s => s.Append(null!)).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void Append_Empty_SetTextAndCaret()
        {
            using var stubbedWindow = new StubbedWindow();
            bool appended = false;
            const string text = "hello";
            var endCaret = Point.Empty;
            var stubbedTextController = new StubbedConsoleTextController
            {
                AppendString = s =>
                {
                    s.Should().Be(text);
                    appended = true;
                    endCaret = new Point(17, 23);
                },
                ValidateCaretPoint = p => p,
                EndCaretGet = () => endCaret
            };
            var size = new Size(10, 10);
            using var sut = new StubbedTextControl(stubbedWindow, stubbedTextController)
            {
                Parent = stubbedWindow,
                Area = new Rectangle(Point.Empty, size)
            };

            sut.Append(text);
            appended.Should().BeTrue();
            sut.Caret.Should().Be(endCaret);
            sut.Scroll.Should().Be(new Point(8, 14));
        }
    }
}
