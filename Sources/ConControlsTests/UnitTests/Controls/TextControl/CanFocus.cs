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
        public void CanFocus_Get_True()
        {
            using var sut = new StubbedTextControl();
            sut.CanFocus.Should().BeFalse();
        }
        [TestMethod]
        public void CanFocus_Set_EventOnlyOnChange()
        {
            using var sut = new StubbedTextControl();
            int raised = 0;
            sut.CanFocusChanged += OnCanFocusChanged;
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(1); 
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(1);
            sut.CanFocus = false;
            sut.CanFocus.Should().BeFalse();
            raised.Should().Be(2);
            sut.CanFocus = false;
            sut.CanFocus.Should().BeFalse();
            raised.Should().Be(2);
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(3);

            sut.CanFocusChanged -= OnCanFocusChanged;
            sut.CanFocus = false;
            raised.Should().Be(3);

            void OnCanFocusChanged(object sender, EventArgs e) => raised++;
        }
    }
}
