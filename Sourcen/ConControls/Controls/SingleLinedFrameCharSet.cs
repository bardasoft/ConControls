/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace ConControls.Controls 
{
    /// <summary>
    /// A <see cref="FrameCharSet"/> for single-lined frames.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SingleLinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a single-lined frame.
        /// </summary>
        public override char UpperLeft { get; } = (char)0x250C;
        /// <summary>
        /// The upper right corner of a single-lined frame.
        /// </summary>
        public override char UpperRight { get; } = (char)0x2510;
        /// <summary>
        /// The lower left corner of a single-lined frame.
        /// </summary>
        public override char LowerLeft { get; } = (char)0x2514;
        /// <summary>
        /// The lower right corner of a single-lined frame.
        /// </summary>
        public override char LowerRight { get; } = (char)0x2518;
        /// <summary>
        /// The horizontal line of a single-lined frame.
        /// </summary>
        public override char Horizontal { get; } = (char)0x2500;
        /// <summary>
        /// The vertical line of a single-lined frame.
        /// </summary>
        public override char Vertical { get; } = (char)0x2502;
    }
}
