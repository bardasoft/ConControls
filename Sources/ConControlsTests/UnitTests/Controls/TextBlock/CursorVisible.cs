/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextBlock
{
    public partial class TextBlockTests
    {
        [TestMethod]
        public void CursorVisible_Get_True()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                Size = new Size(10, 10)
            };
            sut.CursorVisible.Should().BeTrue();
        }
        [TestMethod]
        public void CursorVisible_Set_EventOnlyOnChange()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController)
            {
                Size = (10,10).Sz()
            };
            int raised = 0;
            sut.CursorVisibleChanged += (sender, e) => raised++;
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(1); // base value is false
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(1);
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();
            raised.Should().Be(2);
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();
            raised.Should().Be(2);
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            raised.Should().Be(3);
        }
    }
}
