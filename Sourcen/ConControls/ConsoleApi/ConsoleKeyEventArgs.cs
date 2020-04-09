/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi {
    sealed class ConsoleKeyEventArgs : EventArgs
    {
        public bool KeyDown { get; }
        public ushort RepeatCount { get; }
        public VirtualKeys VirtualKeyCode { get; }
        public ushort VirtualScanCode { get; }
        public char UnicodeChar { get; }
        public ControlKeyStates ControlKeys { get; }
        public ConsoleKeyEventArgs(KEY_EVENT_RECORD rec)
        {
            KeyDown = rec.KeyDown != 0;
            RepeatCount = rec.RepeatCount;
            VirtualKeyCode = rec.VirtualKeyCode;
            VirtualScanCode = rec.VirtualScanCode;
            UnicodeChar = rec.UnicodeChar;
            ControlKeys = rec.ControlKeys;
        }
    }
}
