/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [ExcludeFromCodeCoverage]
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    internal struct KEY_EVENT_RECORD
    {
        [FieldOffset(0)]
        public int KeyDown;
        [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
        public ushort RepeatCount;
        [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
        public VirtualKeys VirtualKeyCode;
        [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
        public ushort VirtualScanCode;
        [FieldOffset(10)]
        public char UnicodeChar;
        [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
        public ControlKeyStates ControlKeys;
    }
}
