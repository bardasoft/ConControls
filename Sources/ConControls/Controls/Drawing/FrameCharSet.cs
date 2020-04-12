/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.Controls.Drawing 
{
    /// <summary>
    /// Provides characters for drawing frames.
    /// </summary>
    /// <remarks>
    /// A <see cref="FrameCharSet"/> represents a character set to use to draw frames
    /// on the console screen buffer. Each <see cref="FrameCharSet"/> represents the characters
    /// used for a <see cref="BorderStyle"/>.<br/>
    /// You can inherit a class from <see cref="FrameCharSets"/> that provides <see cref="FrameCharSet"/> instances
    /// for given <see cref="BorderStyle"/> values. This <see cref="FrameCharSets"/> implementation can then be passed
    /// to <see cref="IConsoleWindow.FrameCharSets">IConsoleWindow.FrameCharSet</see>.
    /// </remarks>
    /// <seealso cref="FrameCharSet"/>
    /// <seealso cref="IConsoleWindow.FrameCharSets"/>
    public abstract class FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a frame.
        /// </summary>
        public abstract char UpperLeft { get; }
        /// <summary>
        /// The upper right corner of a frame.
        /// </summary>
        public abstract char UpperRight { get; }
        /// <summary>
        /// The lower left corner of a frame.
        /// </summary>
        public abstract char LowerLeft { get; }
        /// <summary>
        /// The lower right corner of a frame.
        /// </summary>
        public abstract char LowerRight { get; }
        /// <summary>
        /// The horizontal line of a frame.
        /// </summary>
        public abstract char Horizontal { get; }
        /// <summary>
        /// The vertical line of a frame.
        /// </summary>
        public abstract char Vertical { get; }
    }
}
