/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.WindowsApi
{
    [Flags]
    enum MouseEventFlags: uint
    {
        Moved = 1 << 0,
        DoubleClick = 1 << 1,
        Wheeled = 1 << 2,
        WheeledHorizontally = 1 << 3
    }
}
