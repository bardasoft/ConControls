/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using ConControls.Controls.Drawing;
using ConControls.Controls.Drawing.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void Constructor_WindowInitialized()
        {
            bool cursorSet = false, cursorReset = false;
            bool disposing = false, controllerDisposed = false;
            var consoleController = new StubbedConsoleController
            {
                Dispose = () => controllerDisposed = true
            };
            const int originalCursorSize = 10;
            Size windowSize = new Size(12, 42);
            using var api = new StubbedNativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle =>
                {
                    handle.Should().Be(consoleController.OutputHandle);
                    return new CONSOLE_SCREEN_BUFFER_INFOEX
                    {
                        Window = new SMALL_RECT(windowSize)
                    };
                },
                GetCursorInfoConsoleOutputHandle = handle =>
                {
                    handle.Should().Be(consoleController.OutputHandle);
                    return (true, originalCursorSize, Point.Empty);
                },
                SetCursorInfoConsoleOutputHandleBooleanInt32Point = (handle, visible, size, position) =>
                {
                    handle.Should().Be(consoleController.OutputHandle);
                    if (disposing)
                    {
                        cursorSet.Should().BeTrue();
                        cursorReset.Should().BeFalse();
                        visible.Should().BeTrue();
                        size.Should().Be(originalCursorSize);
                        position.Should().Be(Point.Empty);
                        cursorReset = true;
                        return;
                    }

                    cursorSet.Should().BeFalse();
                    visible.Should().BeFalse();
                    size.Should().Be(originalCursorSize);
                    position.Should().Be(Point.Empty);
                    cursorSet = true;
                }
            };
            bool drawn = false;
            var graphics = new StubIConsoleGraphics
            {
                DrawBackgroundConsoleColorRectangle = (color, rectangle) =>
                {
                    drawn.Should().BeFalse();
                    color.Should().Be(ConsoleColor.Black);
                    rectangle.Should().Be(new Rectangle(Point.Empty, windowSize));
                    drawn = true;
                }
            };
            var graphicsProvider = new StubIProvideConsoleGraphics
            {
                ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, consoleApi, size, frameCharSets) =>
                {
                    handle.Should().Be(consoleController.OutputHandle);
                    consoleApi.Should().Be(api);
                    size.Should().Be(windowSize);
                    frameCharSets.Should().BeOfType<FrameCharSets>();
                    return graphics;
                }
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);
            consoleController.FocusEventEvent.Should().NotBeNull();
            consoleController.KeyEventEvent.Should().NotBeNull();
            consoleController.MenuEventEvent.Should().NotBeNull();
            consoleController.MouseEventEvent.Should().NotBeNull();
            consoleController.SizeEventEvent.Should().NotBeNull();

            sut.Controls.Should().NotBeNull();
            cursorSet.Should().BeTrue();

            drawn.Should().BeTrue();

            controllerDisposed.Should().BeFalse();
            disposing = true;
            sut.Dispose();
            consoleController.FocusEventEvent.Should().BeNull();
            consoleController.KeyEventEvent.Should().BeNull();
            consoleController.MenuEventEvent.Should().BeNull();
            consoleController.MouseEventEvent.Should().BeNull();
            consoleController.SizeEventEvent.Should().BeNull();
            cursorReset.Should().BeTrue();
            controllerDisposed.Should().BeTrue();
        }
    }
}
