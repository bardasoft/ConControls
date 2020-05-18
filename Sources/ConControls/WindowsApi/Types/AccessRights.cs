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

    enum AccessRights : uint
    {
        GenericRead = 0x80000000,
        GenericWrite = 0x40000000
    }
}
