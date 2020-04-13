/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;

namespace ConControls.Controls 
{
    /// <summary>
    /// Arguments for the <see cref="IConsoleWindow.MouseEvent">IConsoleWindow.MouseEvent</see>.
    /// </summary>
    public sealed class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// The state of the control keys. This can be a combination
        /// of <see cref="ControlKeyStates"/> values.
        /// </summary>
        public ControlKeyStates ControlKeys { get; }
        /// <summary>
        /// Indicates which mouse buttons are pressed.
        /// </summary>
        public MouseButtonStates ButtonState { get; }
        /// <summary>
        /// The kind of mouse event. This can be a combination of <see cref="MouseEventFlags"/> values.
        /// </summary>
        /// <remarks>
        /// If this is <see cref="MouseEventFlags.Wheeled"/> or <see cref="MouseEventFlags.WheeledHorizontally"/>
        /// see the <see cref="Scroll"/> value for the amount and direction of the mouse wheel rotation.</remarks>
        public MouseEventFlags Kind { get; }
        /// <summary>
        /// The mouse position, in terms of the console screen buffer's character-cell coordinates.
        /// </summary>
        public Point Position { get; }
        /// <summary>
        /// Returns the amount and direction the mouse wheel has been rotated. This is only valid if <see cref="Kind"/>
        /// is <see cref="MouseEventFlags.Wheeled"/> or <see cref="MouseEventFlags.WheeledHorizontally"/>.
        /// </summary>
        /// <remarks>
        /// If this value is positive, the wheel was rotated forward (away from the user) or to the right (depending on <see cref="Kind"/>).<br/>
        /// If the value is negative, the wheel was rotated backward (toward the user) or to the left.</remarks>
        public int Scroll { get; }
        internal MouseEventArgs(ConsoleMouseEventArgs e)
        {
            ControlKeys = e.ControlKeys;
            ButtonState = e.ButtonState;
            Kind = e.EventFlags;
            Position = e.MousePosition;
            Scroll = e.Scroll;
        }
    }
}
