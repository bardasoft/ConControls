/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void TabStop_InitiallyTrue()
        {
            using var stubbedWindow = new StubbedWindow();
            var controller = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, controller);
            sut.TabStop.Should().BeTrue();
            sut.TabStop = false;
            sut.TabStop.Should().BeFalse();
            sut.TabStop = true;
            sut.TabStop.Should().BeTrue();
        }
    }
}
