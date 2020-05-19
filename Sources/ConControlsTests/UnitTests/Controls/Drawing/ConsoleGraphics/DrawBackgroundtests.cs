/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Linq;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.Drawing.ConsoleGraphics
{
    public partial class ConsoleGraphicsTests
    {
        [TestMethod]
        public void DrawBackground_CorrectArea_FilledCorrectly()
        {
            const char expectedCharacter = default;
            Size size = new Size(4, 4);
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO c0 = new CHAR_INFO
            {
                Attributes = background.ToBackgroundColor() | background.ToForegroundColor(),
                Char = expectedCharacter
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, c0, c0, cc,
                cc, c0, c0, cc,
                cc, cc, cc, cc
            };

            bool written = false, successful = false;
            using var stubbedApi = new StubbedNativeCalls();
            stubbedApi.ReadConsoleOutputConsoleOutputHandleRectangle = (handle, rectangle) =>
            {
                rectangle.Size.Should().Be(size);
                handle.Should().Be(stubbedApi.ScreenHandle);
                return mainBuffer;
            };
            stubbedApi.WriteConsoleOutputConsoleOutputHandleCHAR_INFOArrayRectangle = (handle, buffer, area) =>
            {
                written = true;
                handle.Should().Be(stubbedApi.ScreenHandle);
                area.Size.Should().Be(size);
                buffer.Should().Equal(expectedBuffer);
                successful = true;
            };
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size,
                                                                       new ConControls.Controls.Drawing.FrameCharSets());
            sut.DrawBackground(
                color: background,
                area: new Rectangle(1, 1, 2, 2));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBackground_AreaTooLarge_ClippedCorrectly()
        {
            const char expectedCharacter = default;
            Size size = new Size(4, 4);
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO c0 = new CHAR_INFO
            {
                Attributes = background.ToBackgroundColor() | background.ToForegroundColor(),
                Char = expectedCharacter
            };
            var expectedBuffer = Enumerable.Repeat(c0, 16).ToArray();

            bool written = false, successful = false;
            using var stubbedApi = new StubbedNativeCalls();
            stubbedApi.ReadConsoleOutputConsoleOutputHandleRectangle = (handle, rectangle) =>
            {
                rectangle.Size.Should().Be(size);
                handle.Should().Be(stubbedApi.ScreenHandle);
                return mainBuffer;
            };
            stubbedApi.WriteConsoleOutputConsoleOutputHandleCHAR_INFOArrayRectangle = (handle, buffer, area) =>
            {
                written = true;
                handle.Should().Be(stubbedApi.ScreenHandle);
                area.Size.Should().Be(size);
                buffer.Should().Equal(expectedBuffer);
                successful = true;
            };
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size,
                                                                       new ConControls.Controls.Drawing.FrameCharSets());
            sut.DrawBackground(
                color: background,
                area: new Rectangle(-1, -1, 7, 7));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
    }
}
