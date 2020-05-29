/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void BorderStyleChanged_CursorUpdated()
        {
            var sut = new StubbedTextControl
            {
                Text = string.Empty.PadLeft(15, ' '),
                Size = new Size(10, 10)
            };
            sut.BorderStyle.Should().BeNull();
            sut.Caret = new Point(9, 0);
            sut.CursorPosition.Should().Be(new Point(9, 0));
            sut.BorderStyle = BorderStyle.Bold;
            sut.BorderStyle.Should().Be(BorderStyle.Bold);
            sut.Caret.Should().Be(new Point(9, 0));
            sut.CursorVisible.Should().BeFalse();
        }
    }
}
