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

namespace ConControlsTests.UnitTests.Controls.TextBlock
{
    public partial class TextBlockTests
    {
        [TestMethod]
        public void CanEdit_Get_False()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController);
            sut.CanEdit.Should().BeFalse();
        }
        [TestMethod]
        public void CanEdit_Set_NotSupportedException()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.TextBlock(stubbedWindow, textController);
            sut.Invoking(s => s.CanEdit = true)
               .Should()
               .Throw<NotSupportedException>()
               .Where(e =>
                          e.Message.Contains(nameof(ConControls.Controls.TextBlock)) &&
                          e.Message.Contains(nameof(ConControls.Controls.TextBlock.CanEdit)));
        }
    }
}
