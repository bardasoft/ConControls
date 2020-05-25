/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void FrameCharSets_SetOnlyOnChange()
        {
            var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            bool graphicsRequested = false;
            graphicsProvider.ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, calls, arg3, arg4) =>
            {
                graphicsRequested = true;
                return graphicsProvider.Graphics;
            };
            
            using var sut = new ConControls.Controls.ConsoleWindow(api, new StubbedConsoleController(), graphicsProvider);

            sut.FrameCharSets.Should().BeOfType<FrameCharSets>();

            var fremeCharSets = new FrameCharSets();
            graphicsRequested = false;
            sut.FrameCharSets = fremeCharSets;
            graphicsRequested.Should().BeTrue();
            sut.FrameCharSets.Should().BeSameAs(fremeCharSets);
            graphicsRequested = false;
            sut.FrameCharSets = fremeCharSets;
            graphicsRequested.Should().BeFalse();
            sut.FrameCharSets = new FrameCharSets();
            graphicsRequested.Should().BeTrue();

            sut.Invoking(s => s.FrameCharSets = null!).Should().Throw<ArgumentNullException>();
        }
    }
}
