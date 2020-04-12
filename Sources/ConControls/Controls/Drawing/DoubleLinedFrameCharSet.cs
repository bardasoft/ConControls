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
    /// A <see cref="FrameCharSet"/> for double-lined frames.
    /// </summary>
    /// <seealso cref="FrameCharSet"/>
    /// <seealso cref="FrameCharSets"/>
    /// <seealso cref="IConsoleWindow.FrameCharSets"/>
    [ExcludeFromCodeCoverage]
    public class DoubleLinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a double-lined frame.
        /// </summary>
        public override char UpperLeft { get; } = (char)0x2554;
        /// <summary>
        /// The upper right corner of a double-lined frame.
        /// </summary>
        public override char UpperRight { get; } = (char)0x2557;
        /// <summary>
        /// The lower left corner of a double-lined frame.
        /// </summary>
        public override char LowerLeft { get; } = (char)0x255A;
        /// <summary>
        /// The lower right corner of a double-lined frame.
        /// </summary>
        public override char LowerRight { get; } = (char)0x255D;
        /// <summary>
        /// The horizontal line of a double-lined frame.
        /// </summary>
        public override char Horizontal { get; } = (char)0x2550;
        /// <summary>
        /// The vertical line of a double-lined frame.
        /// </summary>
        public override char Vertical { get; } = (char)0x2551;
    }
}
