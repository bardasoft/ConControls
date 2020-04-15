/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using System.Linq;
using ConControls.ConsoleApi;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Drawing.ConsoleGraphics
{
    public partial class ConsoleGraphicsTests
    {
        [TestMethod]
        public void FillArea_CorrectArea_FilledCorrectly()
        {
            const ConsoleColor foreground = ConsoleColor.Blue;
            const ConsoleColor background = ConsoleColor.Green;
            const char character = 'X';
            ConCharAttributes attributes = foreground.ToForegroundColor() | background.ToBackgroundColor();
            Size size = new Size(4, 4);
            CHAR_INFO cc = new CHAR_INFO
            {
                Attributes = (ConCharAttributes)0xFFFF,
                Char = (char)0xFFFF
            };
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO c0 = new CHAR_INFO
            {
                Attributes = attributes,
                Char = character
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, c0, c0, cc,
                cc, c0, c0, cc,
                cc, cc, cc, cc
            };

            ConsoleOutputHandle outputHanlde = new ConsoleOutputHandle(IntPtr.Zero);
            bool written = false, successful = false;
            var stubbedApi = new StubINativeCalls
            {
                ReadConsoleOutputConsoleOutputHandleRectangle = (handle, rectangle) =>
                {
                    rectangle.Size.Should().Be(size);
                    handle.Should().Be(outputHanlde);
                    return mainBuffer;
                },
                WriteConsoleOutputConsoleOutputHandleCHAR_INFOArrayRectangle = (handle, buffer, area) =>
                {
                    written = true;
                    handle.Should().Be(outputHanlde);
                    area.Size.Should().Be(size);
                    buffer.Should().Equal(expectedBuffer);
                    successful = true;
                }
            };
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(outputHanlde, stubbedApi, size,
                                                                       new ConControls.Controls.Drawing.FrameCharSets());
            sut.FillArea(
                background: background, 
                foreColor: foreground,
                c: character, 
                area: new Rectangle(1, 1, 2, 2));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void FillArea_AreaTooLarge_ClippedCorrectly()
        {
            const ConsoleColor foreground = ConsoleColor.Blue;
            const ConsoleColor background = ConsoleColor.Green;
            const char character = 'X';
            ConCharAttributes attributes = foreground.ToForegroundColor() | background.ToBackgroundColor();
            Size size = new Size(4, 4);
            CHAR_INFO cc = new CHAR_INFO
            {
                Attributes = (ConCharAttributes)0xFFFF,
                Char = (char)0xFFFF
            };
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO c0 = new CHAR_INFO
            {
                Attributes = attributes,
                Char = character
            };
            var expectedBuffer = Enumerable.Repeat(c0, 16).ToArray();

            ConsoleOutputHandle outputHanlde = new ConsoleOutputHandle(IntPtr.Zero);
            bool written = false, successful = false;
            var stubbedApi = new StubINativeCalls
            {
                ReadConsoleOutputConsoleOutputHandleRectangle = (handle, rectangle) =>
                {
                    rectangle.Size.Should().Be(size);
                    handle.Should().Be(outputHanlde);
                    return mainBuffer;
                },
                WriteConsoleOutputConsoleOutputHandleCHAR_INFOArrayRectangle = (handle, buffer, area) =>
                {
                    written = true;
                    handle.Should().Be(outputHanlde);
                    area.Size.Should().Be(size);
                    buffer.Should().Equal(expectedBuffer);
                    successful = true;
                }
            };
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(outputHanlde, stubbedApi, size,
                                                                       new ConControls.Controls.Drawing.FrameCharSets());
            sut.FillArea(
                background: background,
                foreColor: foreground,
                c: character,
                area: new Rectangle(-1, -1, 7, 7));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
    }
}
