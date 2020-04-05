/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Runtime.InteropServices;

namespace ConControls.WindowsApi
{
    [StructLayout(LayoutKind.Sequential)]
    struct COORD
    {
        public ushort X;
        public ushort Y;
    }
}
