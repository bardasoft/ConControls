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
        public void Construction_True_ValuesSet()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab, true, true, true);
            sut.Key.Should().Be(VirtualKey.Tab);
            sut.Alt.Should().BeTrue();
            sut.Ctrl.Should().BeTrue();
            sut.Shift.Should().BeTrue();
        }
        [TestMethod]
        public void Construction_False_ValuesSet()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.Tab, false, false, false);
            sut.Key.Should().Be(VirtualKey.Tab);
            sut.Alt.Should().BeFalse();
            sut.Ctrl.Should().BeFalse();
            sut.Shift.Should().BeFalse();
        }
    }
}
