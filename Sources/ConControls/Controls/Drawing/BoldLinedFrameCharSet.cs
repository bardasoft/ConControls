/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace ConControls.Controls.Drawing 
{
    /// <summary>
    /// A <see cref="FrameCharSet"/> for bold-lined frames.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BoldinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a bold-lined frame.
        /// </summary>
        public override char UpperLeft { get; } = (char)0x250F;
        /// <summary>
        /// The upper right corner of a bold-lined frame.
        /// </summary>
        public override char UpperRight { get; } = (char)0x2513;
        /// <summary>
        /// The lower left corner of a bold-lined frame.
        /// </summary>
        public override char LowerLeft { get; } = (char)0x2517;
        /// <summary>
        /// The lower right corner of a bold-lined frame.
        /// </summary>
        public override char LowerRight { get; } = (char)0x251B;
        /// <summary>
        /// The horizontal line of a bold-lined frame.
        /// </summary>
        public override char Horizontal { get; } = (char)0x2501;
        /// <summary>
        /// The vertical line of a bold-lined frame.
        /// </summary>
        public override char Vertical { get; } = (char)0x2503;
    }
}
