/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Linq;
using ConControls.Controls;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.Drawing.ConsoleGraphics
{
    public partial class ConsoleGraphicsTests
    {
        [TestMethod]
        public void DrawBorder_CorrectArea_FilledCorrectly()
        {
            Size size = new Size(5, 5);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 25).ToArray();
            CHAR_INFO ul = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperLeft
            };
            CHAR_INFO ve = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Vertical
            };
            CHAR_INFO ll = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerLeft
            };
            CHAR_INFO ho = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Horizontal
            };
            CHAR_INFO lr = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerRight
            };
            CHAR_INFO ur = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperRight
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc, cc,
                cc, ul, ho, ur, cc,
                cc, ve, cc, ve, cc,
                cc, ll, ho, lr, cc, 
                cc, cc, cc, cc, cc
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(1, 1, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_BorderStyleNone_Unchanged()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.None;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            var expectedBuffer = Enumerable.Repeat(cc, 16).ToArray();

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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(1, 1, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_OutOfBoundsUpperLeft_ClippedCorrectly()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO ve = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Vertical
            };
            CHAR_INFO ho = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Horizontal
            };
            CHAR_INFO lr = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerRight
            };
            var expectedBuffer = new[]
            {
                cc, ve, cc, cc,
                ho, lr, cc, cc,
                cc, cc, cc, cc,
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(-1, -1, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_OutOfBoundsUpperRight_ClippedCorrectly()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO ve = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Vertical
            };
            CHAR_INFO ll = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerLeft
            };
            CHAR_INFO ho = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Horizontal
            };
            var expectedBuffer = new[]
            {
                cc, cc, ve, cc,
                cc, cc, ll, ho,
                cc, cc, cc, cc,
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(2, -1, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_OutOfBoundsLowerLeft_ClippedCorrectly()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO ve = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Vertical
            };
            CHAR_INFO ho = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Horizontal
            };
            CHAR_INFO ur = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperRight
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, cc, cc, cc,
                ho, ur, cc, cc,
                cc, ve, cc, cc
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(-1, 2, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_OutOfBoundsLowerRight_ClippedCorrectly()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO ul = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperLeft
            };
            CHAR_INFO ve = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Vertical
            };
            CHAR_INFO ho = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.Horizontal
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, cc, cc, cc,
                cc, cc, ul, ho,
                cc, cc, ve, cc
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(2, 2, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_NoClientArea_FilledCorrectly()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();
            var frameCharSet = frameCharSets[borderStyle];

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            CHAR_INFO ul = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperLeft
            };
            CHAR_INFO ll = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerLeft
            };
            CHAR_INFO lr = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.LowerRight
            };
            CHAR_INFO ur = new CHAR_INFO
            {
                Attributes = attributes,
                Char = frameCharSet.UpperRight
            };
            var expectedBuffer = new[]
            {
                cc, cc, cc, cc,
                cc, ul, ur, cc,
                cc, ll, lr, cc,
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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(1, 1, 2, 2),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
        [TestMethod]
        public void DrawBorder_CompletelyOutside_NoChange()
        {
            Size size = new Size(4, 4);
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            var frameCharSets = new ConControls.Controls.Drawing.FrameCharSets();

            var mainBuffer = Enumerable.Repeat(cc, 16).ToArray();
            var expectedBuffer = Enumerable.Repeat(cc, 16).ToArray();

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
            var sut = new ConControls.Controls.Drawing.ConsoleGraphics(stubbedApi.ScreenHandle, stubbedApi, size, frameCharSets);
            sut.DrawBorder(
                background: background,
                foreground: foreground,
                area: new Rectangle(-10, -10, 3, 3),
                style: borderStyle);

            written.Should().BeFalse();
            mainBuffer.Should().Equal(expectedBuffer);
            sut.Flush();
            written.Should().BeTrue();
            successful.Should().BeTrue();
        }
    }
}
