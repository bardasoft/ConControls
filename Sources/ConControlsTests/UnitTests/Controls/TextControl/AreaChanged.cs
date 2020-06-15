/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void AreaChanged_CursorUpdated()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedTextControl(stubbedWindow)
            {
                Text = new string(Enumerable.Repeat(' ', 1000).ToArray()),
                Size = new Size(10, 10),
                Caret = new Point(9, 9)
            };
            
            sut.CursorVisible.Should().BeFalse();
            sut.CursorVisible = true;

            sut.Caret.Should().Be(new Point(9, 9));
            sut.CursorPosition.Should().Be(new Point(9, 9));
            sut.CursorVisible.Should().BeTrue();
            sut.Size.Should().Be(new Size(10, 10));

            sut.Size = new Size(5, 5);
            sut.Size.Should().Be(new Size(5, 5));
            sut.Caret.Should().Be(new Point(9, 9));
            sut.CursorVisible.Should().BeFalse();
        }
    }
}
