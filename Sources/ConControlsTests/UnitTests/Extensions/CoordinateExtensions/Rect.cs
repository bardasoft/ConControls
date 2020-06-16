/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Extensions.CoordinateExtensions
{
    public partial class CoordinateExtensionsTests
    {
        [TestMethod]
        public void Rect_AllCoordinates_CorrectRectangleCreated()
        {
            const int x = 12;
            const int y = 23;
            const int width = 42;
            const int height = 17;

            var result = (x, y, width, height).Rect();
            result.X.Should().Be(x);
            result.Left.Should().Be(x);
            result.Y.Should().Be(y);
            result.Top.Should().Be(y);
            result.Width.Should().Be(width);
            result.Height.Should().Be(height);
        }
        [TestMethod]
        public void Rect_LocationWidthHeight_CorrectRectangleCreated()
        {
            const int x = 12;
            const int y = 23;
            const int width = 42;
            const int height = 17;

            Point location = new Point(x, y);

            var result = (location, width, height).Rect();
            result.X.Should().Be(x);
            result.Left.Should().Be(x);
            result.Y.Should().Be(y);
            result.Top.Should().Be(y);
            result.Width.Should().Be(width);
            result.Height.Should().Be(height);
        }
        [TestMethod]
        public void Rect_LeftTopSize_CorrectRectangleCreated()
        {
            const int x = 12;
            const int y = 23;
            const int width = 42;
            const int height = 17;

            Size size = new Size(width, height);

            var result = (x, y, size).Rect();
            result.X.Should().Be(x);
            result.Left.Should().Be(x);
            result.Y.Should().Be(y);
            result.Top.Should().Be(y);
            result.Width.Should().Be(width);
            result.Height.Should().Be(height);
        }
        [TestMethod]
        public void Rect_LocationSize_CorrectRectangleCreated()
        {
            const int x = 12;
            const int y = 23;
            const int width = 42;
            const int height = 17;

            Point location = new Point(x, y);
            Size size = new Size(width, height);

            var result = (location, size).Rect();
            result.X.Should().Be(x);
            result.Left.Should().Be(x);
            result.Y.Should().Be(y);
            result.Top.Should().Be(y);
            result.Width.Should().Be(width);
            result.Height.Should().Be(height);
        }
    }
}
