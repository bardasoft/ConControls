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
        public void WithAlt_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab);
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeFalse();
            sut = sut.WithAlt();
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeFalse();
        }
        [TestMethod]
        public void WithCtrl_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab);
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeFalse();
            sut = sut.WithCtrl();
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeFalse();
        }
        [TestMethod]
        public void WithShift_Set()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab);
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeFalse();
            sut = sut.WithShift();
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeTrue();
        }
    }
}
