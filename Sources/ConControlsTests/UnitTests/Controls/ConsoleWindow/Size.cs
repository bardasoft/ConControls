/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using ConControls.ConsoleApi.Fakes;
using ConControls.Controls.Drawing.Fakes;
using ConControls.WindowsApi.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void Size_Get_BufferSize()
        {
            var consoleListener = new StubIConsoleController();
            var windowSize = new Size(12, 34);
            var api = new StubINativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX { Window = new SMALL_RECT(windowSize) }
            };
            var graphicsProvider = new StubIProvideConsoleGraphics
            {
                ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, consoleApi, size, frameCharSets) => new StubIConsoleGraphics()
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            sut.Size.Should().Be(windowSize);
        }
        [TestMethod]
        public void Size_Set_NotSupportedException()
        {
            var consoleListener = new StubIConsoleController();
            var windowSize = new Size(12, 34);
            var api = new StubINativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX { Window = new SMALL_RECT(windowSize) }
            };
            var graphicsProvider = new StubIProvideConsoleGraphics
            {
                ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, consoleApi, size, frameCharSets) => new StubIConsoleGraphics()
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            sut.Invoking(s => s.Size = new Size(10, 10)).Should().Throw<NotSupportedException>();
        }
    }
}
