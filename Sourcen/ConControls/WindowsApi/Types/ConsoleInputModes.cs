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
    enum ConsoleInputModes
    {
        EnableProcessedInput = 1 << 0,
        EnableLineInput = 1 << 1,
        EnableEchoInput = 1 << 2,
        EnableWindowInput = 1 << 3,
        EnableMouseInput = 1 << 4,
        EnableInsertMode = 1 << 5,
        EnableQuickEditMode = 1 << 6,
        EnableExtendedFlags = 1 << 7,
        EnableAutoPosition = 1 << 8
    }
}
