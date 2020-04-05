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
    enum MouseButtonStates
    {
        LeftButtonPressed = 1 << 0,
        RightButtonPressed = 1 << 1,
        SecondButtonPressed = 1 << 2,
        ThirdButtonPressed = 1 << 3,
        FourthButtonPressed = 1 << 4
    }
}
