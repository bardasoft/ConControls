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
        public void Size_Get_BufferSize()
        {
            var consoleListener = new StubbedConsoleController();
            var windowSize = new Size(12, 34);
            using var api = new StubbedNativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle => new CONSOLE_SCREEN_BUFFER_INFOEX { Window = new SMALL_RECT(1,2,3,4), BufferSize = new COORD(windowSize) }
            };
            var graphicsProvider = new StubbedGraphicsProvider();

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            sut.Size.Should().Be(windowSize);
        }
        [TestMethod]
        public void Size_SmallToLarge_CorrectTranisition()
        {
            PerformSizeTest(
                original: (5, 5).Sz(),
                target: (10, 10).Sz(),
                maximum: (20, 20).Sz(),
                expectedTempWindowSize: (5, 5).Sz(),
                expectedFinalWindowSize: (10, 10).Sz());
        }
        [TestMethod]
        public void Size_SmallToLargerThanMaximum_CorrectTranisition()
        {
            PerformSizeTest(
                original: (5, 5).Sz(),
                target: (10, 10).Sz(),
                maximum: (9, 8).Sz(),
                expectedTempWindowSize: (5, 5).Sz(),
                expectedFinalWindowSize: (9, 8).Sz());
        }
        [TestMethod]
        public void Size_LargeToSmall_CorrectTransisiton()
        {
            PerformSizeTest(
                original: (10, 10).Sz(),
                target: (5, 5).Sz(),
                maximum: (20, 20).Sz(),
                expectedTempWindowSize: (5, 5).Sz(),
                expectedFinalWindowSize: (5, 5).Sz());
        }
        [TestMethod]
        public void Size_LongToWide_CorrectTransisiton()
        {
            PerformSizeTest(
                original: (5, 20).Sz(),
                target: (20, 5).Sz(),
                maximum: (25, 25).Sz(),
                expectedTempWindowSize: (5, 5).Sz(),
                expectedFinalWindowSize: (20, 5).Sz());
        }
        [TestMethod]
        public void Size_WideToLong_CorrectTransisiton()
        {
            PerformSizeTest(
                original: (20, 5).Sz(),
                target: (5, 20).Sz(),
                maximum: (25, 25).Sz(),
                expectedTempWindowSize: (5, 5).Sz(),
                expectedFinalWindowSize: (5, 20).Sz());
        }

        static void PerformSizeTest(Size original, Size target, Size maximum, Size expectedTempWindowSize, Size expectedFinalWindowSize)
        {
            Size currentWindowSize = original, currentBufferSize = original;
            int windowSet = 0;

            using var api = new StubbedNativeCalls();
            var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);

            api.GetConsoleScreenBufferInfoConsoleOutputHandle = handle =>
            {
                handle.Should().Be(controller.OutputHandle);
                return new CONSOLE_SCREEN_BUFFER_INFOEX
                {
                    Window = new SMALL_RECT(currentWindowSize),
                    BufferSize = new COORD(currentBufferSize),
                    MaximumWindowSize = new COORD(maximum)
                };
            };
            api.SetConsoleWindowSizeConsoleOutputHandleSize = (handle, size) =>
            {
                handle.Should().Be(controller.OutputHandle);
                currentWindowSize = size;
                if (windowSet == 0)
                {
                    size.Should().Be(expectedTempWindowSize);
                    windowSet = 1;
                    return;
                }

                size.Should().Be(expectedFinalWindowSize);
                windowSet.Should().Be(1);
                windowSet = 2;
            };
            api.SetConsoleScreenBufferSizeConsoleOutputHandleSize = (handle, size) =>
            {
                handle.Should().Be(controller.OutputHandle);
                size.Should().Be(target);
                currentBufferSize = size;
            };
            sut.Size.Should().Be(original);

            sut.Size = target;
            currentBufferSize.Should().Be(target);
            currentWindowSize.Should().Be(expectedFinalWindowSize);

        }
    }
}
