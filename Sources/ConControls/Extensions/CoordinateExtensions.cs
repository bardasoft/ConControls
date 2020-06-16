/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Drawing;

namespace ConControls.Extensions
{
    /// <summary>
    /// Provides extension methods to create <see cref="Point"/>,
    /// <see cref="Size"/> or <see cref="Rectangle"/> instances from tuples.
    /// </summary>
    public static class CoordinateExtensions
    {
        /// <summary>
        /// Creates a <see cref="Point"/> instance from the given coordinates.
        /// </summary>
        /// <param name="p">A tuple of two integers representing the <c>x</c> and <c>y</c> coordinates.</param>
        /// <returns>A <see cref="Point"/> with its <see cref="Point.X"/> and <see cref="Point.Y"/> properties set to the given coordinates.</returns>
        public static Point Pt(this (int x, int y) p) => new Point(p.x, p.y);
        /// <summary>
        /// Creates a <see cref="Size"/> instance from the given values.
        /// </summary>
        /// <param name="p">A tuple of two integers representing the <c>width</c> and <c>height</c> values.</param>
        /// <returns>A <see cref="Size"/> with its <see cref="Size.Width"/> and <see cref="Size.Height"/> properties set to the given values.</returns>
        public static Size Sz(this (int width, int height) p) => new Size(p.width, p.height);
        /// <summary>
        /// Creates a <see cref="Rectangle"/> instance from the given values.
        /// </summary>
        /// <param name="r">A tuple of four integers representing the
        /// <c>left</c>, <c>top</c>, <c>width</c> and <c>height</c> values.</param>
        /// <returns>A <see cref="Rectangle"/> with its properties set to the given values.</returns>
        public static Rectangle Rect(this (int left, int top, int width, int height) r) => new Rectangle(r.left, r.top, r.width, r.height);
        /// <summary>
        /// Creates a <see cref="Rectangle"/> instance from the given values.
        /// </summary>
        /// <param name="r">A tuple of two integers (representing the
        /// <c>left</c> and <c>top</c> values) and a <see cref="Size"/>
        /// value representing the size of the rectangle to create.</param>
        /// <returns>A <see cref="Rectangle"/> with its properties set to the given values.</returns>
        public static Rectangle Rect(this (int left, int top, Size size) r) => new Rectangle((r.left, r.top).Pt(), r.size);
        /// <summary>
        /// Creates a <see cref="Rectangle"/> instance from the given values.
        /// </summary>
        /// <param name="r">A tuple of a <see cref="Point"/> value (representing
        /// the rectangle's position) and two integers representing the
        /// <c>width</c> and <c>height</c> values.</param>
        /// <returns>A <see cref="Rectangle"/> with its properties set to the given values.</returns>
        public static Rectangle Rect(this (Point location, int width, int height) r) => new Rectangle(r.location, (r.width, r.height).Sz());
        /// <summary>
        /// Creates a <see cref="Rectangle"/> instance from the given values.
        /// </summary>
        /// <param name="r">A tuple of a <see cref="Point"/> value (representing
        /// the rectangle's position) and a <see cref="Size"/> value (representing
        /// the rectangle's size).</param>
        /// <returns>A <see cref="Rectangle"/> with its properties set to the given values.</returns>
        public static Rectangle Rect(this (Point location, Size size) r) => new Rectangle(r.location, r.size);
    }
}
