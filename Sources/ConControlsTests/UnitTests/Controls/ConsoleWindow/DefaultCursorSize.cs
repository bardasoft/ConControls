/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void DefaultCursorSize_SetAndDrawn()
        {
            const int originalCursorSize = 17;
            const int alternativeCursorSize = 23;
            var api = new StubbedNativeCalls
            {
                GetCursorInfoConsoleOutputHandle = handle => (true, originalCursorSize, Point.Empty)
            };
            using var controller = new StubbedConsoleController();
            bool graphicsProvided = false;
            var graphicsProvider = new StubbedGraphicsProvider();
            graphicsProvider.ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, calls, arg3, arg4) =>
            {
                graphicsProvided = true;
                return graphicsProvider.Graphics;
            };
            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            var suti = (IControlContainer)sut;
            sut.DefaultCursorSize.Should().Be(originalCursorSize);
            suti.CursorSize.Should().Be(originalCursorSize);
            graphicsProvided = false;
            suti.CursorSize = alternativeCursorSize;
            graphicsProvided.Should().BeTrue();
            sut.DefaultCursorSize.Should().Be(alternativeCursorSize);

            graphicsProvided = false;
            suti.CursorSize = null;
            graphicsProvided.Should().BeFalse();
            sut.DefaultCursorSize.Should().Be(alternativeCursorSize);
        }
    }
}
