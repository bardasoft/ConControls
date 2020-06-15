/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextBlock
{
    public partial class TextBlockTests
    {
        [TestMethod]
        public void TabStop_Get_True()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController);
            sut.TabStop.Should().BeTrue();
        }
        [TestMethod]
        public void TabStop_Set_Set()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                TabStop = true
            };
            sut.TabStop.Should().BeTrue();
            sut.TabStop = false;
            sut.TabStop.Should().BeFalse();
            sut.TabStop = false;
            sut.TabStop.Should().BeFalse();
            sut.TabStop = true;
            sut.TabStop.Should().BeTrue();
        }
    }
}
