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
    [StructLayout(LayoutKind.Explicit)]
    struct MOUSE_EVENT_RECORD
    {
        [FieldOffset(0)]
        public COORD MousePosition;
        [FieldOffset(4)]
        public MouseButtonStates ButtonState;
        [FieldOffset(6)]
        public short Scroll;
        [FieldOffset(8)]
        public ControlKeyStates ControlKeys;
        [FieldOffset(12)]
        public MouseEventFlags EventFlags;
    }
}
