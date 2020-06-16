/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void Area_PointEmptyAndSize()
        {
            Size windowSize = (5, 7).Sz();
            var api = new StubbedNativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX
                {
                    BufferSize = new COORD(windowSize)
                }
            };
            using var controller = new StubbedConsoleController();
            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, new StubbedGraphicsProvider());

            sut.Area.Should().Be((Point.Empty, windowSize).Rect());
        }
    }
}
