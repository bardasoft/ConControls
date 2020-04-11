/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.WindowsApi.Types
{
    [Flags]
    enum ControlKeyStates
    {
        RIGHT_ALT_PRESSED = 1 << 0,
        LEFT_ALT_PRESSED = 1 << 1,
        RIGHT_CTRL_PRESSED = 1 << 2,
        LEFT_CTRL_PRESSED = 1 << 3,
        SHIFT_PRESSED = 1 << 4,
        NUMLOCK_ON = 1 << 5,
        SCROLLLOCK_ON = 1 << 6,
        CAPSLOCK_ON = 1 << 7,
        ENHANCED_KEY = 1 << 8
    }
}
