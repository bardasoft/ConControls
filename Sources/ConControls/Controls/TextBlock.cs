/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Drawing;

namespace ConControls.Controls
{
    /// <summary>
    /// A console control to display a scrollable block of text.
    /// </summary>
    public sealed class TextBlock : ConsoleControl
    {
        /// <summary>
        /// Returns <c>true</c> for a <see cref="TextBlock"/>. This control can be focused.
        /// </summary>
        public override bool CanFocus => true;

        /// <inheritdoc />
        public TextBlock(IControlContainer parent)
            : base(parent)
        {
            CursorSize = Window.CursorSize;
            CursorVisible = true;
            CursorPosition = new Point(0, 0);
        }
    }
}
