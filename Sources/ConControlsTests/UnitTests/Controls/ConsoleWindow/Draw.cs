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
        public void Draw_NotDrawnWhenInhibited()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();
            bool provided = false;
            graphicsProvider.ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, calls, arg3, arg4) =>
            {
                provided = true;
                return graphicsProvider.Graphics;
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            provided = false;
            using(sut.DeferDrawing())
            {
                sut.Invalidate();
                provided.Should().BeFalse();
            }

            provided.Should().BeTrue();
        }
    }
}
