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
using ConControls.Controls.Drawing;
using ConControls.Controls.Drawing.Fakes;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void Constructor_WindowInitialized()
        {
            bool cursorSet = false, sizeSet = false, cursorReset = false;
            bool disposing = false, listenerDisposed = false;
            var outputHandle = new ConsoleOutputHandle(IntPtr.Zero);
            var consoleListener = new StubIConsoleController
            {
                OriginalOutputHandleGet = () => outputHandle,
                Dispose = () => listenerDisposed = true
            };
            const int originalCursorSize = 10;
            Size windowSize = new Size(12, 42);
            var api = new StubINativeCalls
            {
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle =>
                {
                    handle.Should().Be(outputHandle);
                    return new CONSOLE_SCREEN_BUFFER_INFOEX
                    {
                        Window = new SMALL_RECT(windowSize)
                    };
                },
                GetCursorInfoConsoleOutputHandle = handle =>
                {
                    handle.Should().Be(outputHandle);
                    return (true, originalCursorSize, Point.Empty);
                },
                SetConsoleScreenBufferSizeConsoleOutputHandleCOORD = (handle, size) =>
                {
                    sizeSet.Should().BeFalse();
                    handle.Should().Be(outputHandle);
                    size.Should().Be(new COORD(windowSize));
                    sizeSet = true;
                }
                ,
                SetCursorInfoConsoleOutputHandleBooleanInt32Point = (handle, visible, size, position) =>
                {
                    handle.Should().Be(outputHandle);
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
                    handle.Should().Be(outputHandle);
                    consoleApi.Should().Be(api);
                    size.Should().Be(windowSize);
                    frameCharSets.Should().BeOfType<FrameCharSets>();
                    return graphics;
                }
            };

            var sut = new ConControls.Controls.ConsoleWindow(api, consoleListener, graphicsProvider);
            consoleListener.OutputReceivedEvent.Should().NotBeNull();
            consoleListener.ErrorReceivedEvent.Should().NotBeNull();
            consoleListener.FocusEventEvent.Should().NotBeNull();
            consoleListener.KeyEventEvent.Should().NotBeNull();
            consoleListener.MenuEventEvent.Should().NotBeNull();
            consoleListener.MouseEventEvent.Should().NotBeNull();
            consoleListener.SizeEventEvent.Should().NotBeNull();

            sut.Controls.Should().NotBeNull();
            cursorSet.Should().BeTrue();
            sizeSet.Should().BeTrue();

            drawn.Should().BeTrue();

            listenerDisposed.Should().BeFalse();
            disposing = true;
            sut.Dispose();
            consoleListener.OutputReceivedEvent.Should().BeNull();
            consoleListener.ErrorReceivedEvent.Should().BeNull();
            consoleListener.FocusEventEvent.Should().BeNull();
            consoleListener.KeyEventEvent.Should().BeNull();
            consoleListener.MenuEventEvent.Should().BeNull();
            consoleListener.MouseEventEvent.Should().BeNull();
            consoleListener.SizeEventEvent.Should().BeNull();
            cursorReset.Should().BeTrue();
            listenerDisposed.Should().BeTrue();
        }
    }
}
