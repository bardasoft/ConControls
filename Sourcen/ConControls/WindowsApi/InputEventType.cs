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
    enum InputEventType : ushort
    {
        Key = 1 << 0,
        Mouse = 1 << 1,
        WindowBufferSize = 1 << 2,
        Menu = 1 << 3,
        Focus = 1 << 4
    }
}
