/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    static class PointExtensions
    {
        internal static Point Pt(this (int x, int y) p) => new Point(p.x, p.y);
        internal static Size Sz(this (int x, int y) p) => new Size(p.x, p.y);
        internal static Rectangle Rect(this (int left, int top, int width, int height) r) => new Rectangle(r.left, r.top, r.width, r.height);
        internal static Rectangle Rect(this (int left, int top, Size size) r) => new Rectangle((r.left, r.top).Pt(), r.size);
        internal static Rectangle Rect(this (Point location, int width, int height) r) => new Rectangle(r.location, (r.width, r.height).Sz());
        internal static Rectangle Rect(this (Point location, Size size) r) => new Rectangle(r.location, r.size);
    }
}
