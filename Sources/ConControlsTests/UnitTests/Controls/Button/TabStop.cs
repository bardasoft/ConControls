/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void TabStop_InitiallyTrue()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow);
            sut.TabStop.Should().BeTrue();
            sut.TabStop = false;
            sut.TabStop.Should().BeFalse();
            sut.TabStop = true;
            sut.TabStop.Should().BeTrue();
        }
    }
}
