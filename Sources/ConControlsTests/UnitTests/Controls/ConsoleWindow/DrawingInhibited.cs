/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void DrawinInhiibited_DependingOnVisibility()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, new StubbedGraphicsProvider());

            sut.DrawingInhibited.Should().BeFalse();
            using(sut.DeferDrawing())
                sut.DrawingInhibited.Should().BeTrue();
            sut.DrawingInhibited.Should().BeFalse();
            sut.Dispose();
            sut.DrawingInhibited.Should().BeTrue();

        }
    }
}
