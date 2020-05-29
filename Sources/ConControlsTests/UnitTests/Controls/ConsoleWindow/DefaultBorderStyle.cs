/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void DefaultBorderStyle_SetAndDrawn()
        {
            const BorderStyle style = BorderStyle.Bold;
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
            sut.DefaultBorderStyle.Should().Be(BorderStyle.None);
            suti.BorderStyle.Should().Be(BorderStyle.None);
            graphicsProvided = false;
            suti.BorderStyle = style;
            graphicsProvided.Should().BeTrue();
            sut.DefaultBorderStyle.Should().Be(style);

            graphicsProvided = false;
            suti.BorderStyle = null;
            graphicsProvided.Should().BeFalse();
            sut.DefaultBorderStyle.Should().Be(style);
        }
    }
}
