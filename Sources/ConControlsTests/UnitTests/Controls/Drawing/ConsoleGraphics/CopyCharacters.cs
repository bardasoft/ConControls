/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.Drawing.ConsoleGraphics
{
    public partial class ConsoleGraphicsTests
    {
        [TestMethod]
        public void CopyCharacters_CorrectArea_CopiedCorrectly()
        {
            Size size = new Size(4, 4);
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            char[] characters = { cc1, cc2, cc3, cc4 };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, cA, cB, cc,
                cc, cC, cD, cc,
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
            sut.CopyCharacters(
                background: background,
                foreColor: foreground,
                topLeft: new Point(1, 1),
                characters: characters,
                arraySize: new Size(2, 2));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void CopyCharacters_AreaTooLarge_ClippedCorrectly()
        {
            Size size = new Size(4, 4);
            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            char[] characters =
            {
                cc1, cc2, cc3, cc4, cc1,
                cc2, cc3, cc4, cc1, cc2,
                cc3, cc4, cc1, cc2, cc3,
                cc4, cc1, cc2, cc3, cc4,
                cc1, cc2, cc3, cc4, cc1 };
            var expectedBuffer = new[]
            {
                cA, cB, cC, cD,
                cB, cC, cD, cA,
                cC, cD, cA, cB,
                cD, cA, cB, cC
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
            sut.CopyCharacters(
                background: background,
                foreColor: foreground,
                topLeft: Point.Empty,
                characters: characters,
                arraySize: new Size(5, 5));

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
    }
}
