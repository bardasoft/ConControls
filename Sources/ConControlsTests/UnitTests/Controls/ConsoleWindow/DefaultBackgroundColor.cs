/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void DefaultBackgroundColor_SetAndDrawn()
        {
            const ConsoleColor color = ConsoleColor.DarkMagenta;
            var api = new StubbedNativeCalls();
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
            sut.DefaultBackgroundColor.Should().Be(ConsoleColor.Black);
            suti.BackgroundColor.Should().Be(ConsoleColor.Black);
            graphicsProvided = false;
            suti.BackgroundColor = color;
            graphicsProvided.Should().BeTrue();
            sut.DefaultBackgroundColor.Should().Be(color);

            graphicsProvided = false;
            suti.BackgroundColor = null;
            graphicsProvided.Should().BeFalse();
            sut.DefaultBackgroundColor.Should().Be(color);
        }
    }
}
