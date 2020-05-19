/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
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
            var consoleListener = new StubbedConsoleController();
            var windowSize = new Size(12, 34);
            using var api = new StubbedNativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX { Window = new SMALL_RECT(windowSize) }
            };
            var graphicsProvider = new StubbedGraphicsProvider();

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            sut.Size.Should().Be(windowSize);
        }
        [TestMethod]
        public void Size_Set_NotSupportedException()
        {
            var consoleListener = new StubbedConsoleController();
            var windowSize = new Size(12, 34);
            using var api = new StubbedNativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX { Window = new SMALL_RECT(windowSize) }
            };
            var graphicsProvider = new StubbedGraphicsProvider();

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            sut.Invoking(s => s.Size = new Size(10, 10)).Should().Throw<NotSupportedException>();
        }
    }
}
