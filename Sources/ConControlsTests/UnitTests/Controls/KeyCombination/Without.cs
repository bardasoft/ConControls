/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace ConControlsTests.UnitTests.Controls.KeyCombination
{
    public partial class KeyCombinationTests
    {
        [TestMethod]
        public void WithoutoutAlt_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab, true, true, true);
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeTrue();
            sut = sut.WithoutAlt();
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeTrue();
        }
        [TestMethod]
        public void WithoutCtrl_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab, true, true, true);
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeTrue();
            sut = sut.WithoutCtrl();
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeTrue();
        }
        [TestMethod]
        public void WithoutShift_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab, true, true, true);
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeTrue();
            sut = sut.WithoutShift();
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeFalse();
        }
    }
}
