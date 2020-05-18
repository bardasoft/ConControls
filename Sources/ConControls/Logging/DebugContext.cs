/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */
using System;

namespace ConControls.Logging 
{
    [Flags]
    enum DebugContext
    {
        None = 0,

        ConsoleApi = 1 << 0,
        ConsoleListener = 1 << 1,
        Graphics = 1 << 2,
        Window = 1 << 3,
        Control = 1 << 4,
        Drawing = 1 << 5,

        ProgressBar = 1 << 6,
        Text = 1 << 7,

        Exception = 1 << 30,
        All = -1
    }
}
