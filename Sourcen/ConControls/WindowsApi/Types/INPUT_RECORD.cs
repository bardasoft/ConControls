/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct INPUT_RECORD
    {
        [StructLayout(LayoutKind.Explicit)]
        internal struct EVENTUNION
        {
            [FieldOffset(0)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(0)]
            public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(0)]
            public WINDOW_BUFFER_SIZE_RECORD SizeEvent;
            [FieldOffset(0)]
            public MENU_EVENT_RECORD MenuEent;
            [FieldOffset(0)]
            public FOCUS_EVENT_RECORD FocusEvent;
        }

        public InputEventType EventType;
        public EVENTUNION Event;
    }
}
