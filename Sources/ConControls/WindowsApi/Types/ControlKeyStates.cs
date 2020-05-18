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
    /// The state of the control keys.
    /// </summary>
    [Flags]
    public enum ControlKeyStates
    {
        /// <summary>
        /// No modifier key is pressed.
        /// </summary>
        None = 0,
        /// <summary>
        /// The right ALT key is pressed.
        /// </summary>
        RIGHT_ALT_PRESSED = 1 << 0,
        /// <summary>
        /// The left ALT key is pressed.
        /// </summary>
        LEFT_ALT_PRESSED = 1 << 1,
        /// <summary>
        /// The right CTRL key is pressed.
        /// </summary>
        RIGHT_CTRL_PRESSED = 1 << 2,
        /// <summary>
        /// The left CTRL key is pressed.
        /// </summary>
        LEFT_CTRL_PRESSED = 1 << 3,
        /// <summary>
        /// The SHIFT key is pressed.
        /// </summary>
        SHIFT_PRESSED = 1 << 4,
        /// <summary>
        /// The NUM LOCK light is on.
        /// </summary>
        NUMLOCK_ON = 1 << 5,
        /// <summary>
        /// The SCROLL LOCK light is on.
        /// </summary>
        SCROLLLOCK_ON = 1 << 6,
        /// <summary>
        /// The CAPS LOCK light is on.
        /// </summary>
        CAPSLOCK_ON = 1 << 7,
        /// <summary>
        /// The key is enhanced.
        /// </summary>
        ENHANCED_KEY = 1 << 8
    }
}
