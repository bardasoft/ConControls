/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.WindowsApi.Types
{
    /// <summary>
    /// The different kinds of console mouse events.
    /// </summary>
    [Flags]
    public enum MouseEventFlags
    {
        /// <summary>
        /// A change in mouse position occurred.
        /// </summary>
        Moved = 1 << 0,
        /// <summary>
        /// The second click (button press) of a double-click occurred.
        /// The first click is returned as a regular button-press event.
        /// </summary>
        DoubleClick = 1 << 1,
        /// <summary>
        /// The vertical mouse wheel was moved.
        /// </summary>
        Wheeled = 1 << 2,
        /// <summary>
        /// The horizontal mouse wheel was moved.
        /// </summary>
        WheeledHorizontally = 1 << 3
    }
}
