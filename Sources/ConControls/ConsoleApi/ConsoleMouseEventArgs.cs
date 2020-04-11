/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi
{
    [ExcludeFromCodeCoverage]
    sealed class ConsoleMouseEventArgs : EventArgs
    {
        public Point MousePosition { get; }
        public MouseButtonStates ButtonState { get; }
        public short Scroll { get; }
        public ControlKeyStates ControlKeys { get; }
        public MouseEventFlags EventFlags { get; }
        public ConsoleMouseEventArgs(MOUSE_EVENT_RECORD rec)
        {        
            MousePosition = new Point(rec.MousePosition.X, rec.MousePosition.Y);
            ButtonState = rec.ButtonState;
            Scroll = rec.Scroll;
            ControlKeys = rec.ControlKeys;
            EventFlags = rec.EventFlags;
        }
    }
}
