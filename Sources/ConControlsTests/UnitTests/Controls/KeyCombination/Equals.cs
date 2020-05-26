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
        public void Equals_DifferentType_False()
        {
            var sut = new ConControls.Controls.KeyCombination(VirtualKey.A);
            sut.Equals(new object()).Should().BeFalse();
        }
        [TestMethod]
        public void Equals_DifferentKey_False()
        {
            var sut1 = new ConControls.Controls.KeyCombination(VirtualKey.A);
            var sut2 = new ConControls.Controls.KeyCombination(VirtualKey.B);
            sut1.Equals((object)sut2).Should().BeFalse();
            sut1.Equals(sut2).Should().BeFalse();
            (sut1 == sut2).Should().BeFalse();
            (sut1 != sut2).Should().BeTrue();

        }
        [TestMethod]
        public void Equals_DifferentAlt_False()
        {
            var sut1 = new ConControls.Controls.KeyCombination(VirtualKey.A);
            var sut2 = sut1.WithAlt();
            sut1.Equals((object)sut2).Should().BeFalse();
            sut1.Equals(sut2).Should().BeFalse();
            (sut1 == sut2).Should().BeFalse();
            (sut1 != sut2).Should().BeTrue();

        }
        [TestMethod]
        public void Equals_DifferentCtrl_False()
        {
            var sut1 = new ConControls.Controls.KeyCombination(VirtualKey.A);
            var sut2 = sut1.WithCtrl();
            sut1.Equals((object)sut2).Should().BeFalse();
            sut1.Equals(sut2).Should().BeFalse();
            (sut1 == sut2).Should().BeFalse();
            (sut1 != sut2).Should().BeTrue();

        }
        [TestMethod]
        public void Equals_DifferentShift_False()
        {
            var sut1 = new ConControls.Controls.KeyCombination(VirtualKey.A);
            var sut2 = sut1.WithShift();
            sut1.Equals((object)sut2).Should().BeFalse();
            sut1.Equals(sut2).Should().BeFalse();
            (sut1 == sut2).Should().BeFalse();
            (sut1 != sut2).Should().BeTrue();

        }
        [TestMethod]
        public void Equals_Equal_True()
        {
            var sut1 = new ConControls.Controls.KeyCombination(VirtualKey.A, true, true, true);
            var sut2 = new ConControls.Controls.KeyCombination(VirtualKey.A, true, true, true);
            sut1.Equals((object)sut2).Should().BeTrue();
            sut1.Equals(sut2).Should().BeTrue();
            (sut1 == sut2).Should().BeTrue();
            (sut1 != sut2).Should().BeFalse();

        }
    }
}
