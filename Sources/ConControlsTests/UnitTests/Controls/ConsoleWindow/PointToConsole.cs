/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.ConsoleApi.Fakes;
using ConControls.Controls.Drawing.Fakes;
using ConControls.WindowsApi.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void PointToConsole_Identity()
        {
            var consoleListener = new StubIConsoleController();
            var api = new StubINativeCalls();
            var graphicsProvider = new StubIProvideConsoleGraphics
            {
                ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, consoleApi, size, frameCharSets) => new StubIConsoleGraphics()
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            Point p = new Point(12, 34);
            sut.PointToConsole(p).Should().Be(p);
        }
    }
}
