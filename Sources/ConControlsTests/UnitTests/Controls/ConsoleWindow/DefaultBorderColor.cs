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
        public void DefaultBorderColor_SetAndDRawn()
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
            sut.DefaultBorderColor.Should().Be(ConsoleColor.Yellow);
            suti.BorderColor.Should().Be(ConsoleColor.Yellow);
            graphicsProvided = false;
            suti.BorderColor = color;
            graphicsProvided.Should().BeTrue();
            sut.DefaultBorderColor.Should().Be(color);

            graphicsProvided = false;
            suti.BorderColor = null;
            graphicsProvided.Should().BeFalse();
            sut.DefaultBorderColor.Should().Be(color);
        }
    }
}
