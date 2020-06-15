/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void CanEdit_Get_False()
        {
            using var sut = new StubbedTextControl();
            sut.CanEdit.Should().BeFalse();
        }
        [TestMethod]
        public void CanEdit_Set_NotSupportedException()
        {
            using var sut = new StubbedTextControl();
            sut.Invoking(s => s.CanEdit = true)
               .Should()
               .Throw<NotSupportedException>()
               .Where(e =>
                          e.Message.Contains(nameof(ConControls.Controls.TextControl)) &&
                          e.Message.Contains(nameof(ConControls.Controls.TextControl.CanEdit)));
        }
    }
}
