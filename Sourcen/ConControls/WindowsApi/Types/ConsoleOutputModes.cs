/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.WindowsApi.Types {
    [Flags]
    enum ConsoleOutputModes
    {
        None = 0,
        EnableProcessedOutput = 1 << 0,
        EnableWrapAtEolOutput = 1 << 1,
        EnableVirtualTerminalProcessing = 1 << 2,
        DisableNewLineAutoReturn = 1 << 3,
        EnableLvbGridWorldwide = 1 << 4,
    }
}
