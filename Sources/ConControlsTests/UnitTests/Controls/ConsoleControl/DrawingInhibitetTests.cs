/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void DrawingInhibited_DrawingAllowed_False()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DrawingInhibited.Should().BeFalse();
        }
        [TestMethod]
        public void DrawingInhibited_NotVisible_True()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Visible = false
            };
            sut.DrawingInhibited.Should().BeTrue();
        }
        [TestMethod]
        public void DrawingInhibited_ParentInhibited_True()
        {
            var stubbedWindow = new StubbedWindow {DrawingInhibitedGet = () => true};
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DrawingInhibited.Should().BeTrue();
        }
        [TestMethod]
        public void DrawingInhibited_DrawingDeferred_Correct()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            var deferrer1 = sut.DeferDrawing();
            sut.DrawingInhibited.Should().BeTrue();
            var deferrer2 = sut.DeferDrawing();
            sut.DrawingInhibited.Should().BeTrue();
            deferrer1.Dispose();
            sut.DrawingInhibited.Should().BeTrue();
            deferrer2.Dispose();
            sut.DrawingInhibited.Should().BeFalse();
        }
    }
}
