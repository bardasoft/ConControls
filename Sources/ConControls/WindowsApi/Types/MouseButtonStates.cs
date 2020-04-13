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
    /// The states of the mouse buttons.
    /// </summary>
    [Flags]
    public enum MouseButtonStates
    {
        /// <summary>
        /// The leftmost mouse button.
        /// </summary>
        LeftButtonPressed = 1 << 0,
        /// <summary>
        /// The rightmost mouse button.
        /// </summary>
        RightButtonPressed = 1 << 1,
        /// <summary>
        /// The second button from the left.
        /// </summary>
        SecondButtonPressed = 1 << 2,
        /// <summary>
        /// The third button from the left.
        /// </summary>
        ThirdButtonPressed = 1 << 3,
        /// <summary>
        /// The fourth button from the left.
        /// </summary>
        FourthButtonPressed = 1 << 4
    }
}
