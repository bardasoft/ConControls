/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.WindowsApi.Types;

namespace ConControls.Helpers
{
    static class KeyHandlingExtensions
    {
        internal static ControlKeyStates WithoutSwitches(this ControlKeyStates controlKeys) =>
            controlKeys & ~(ControlKeyStates.CAPSLOCK_ON | ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SCROLLLOCK_ON);
        internal static bool Matches(this ConsoleKeyEventArgs e, KeyCombination? combination)
        {
            if (combination == null) return false;
            var combi = new KeyCombination(e.VirtualKeyCode);
            if (e.ControlKeys.HasFlag(ControlKeyStates.LEFT_ALT_PRESSED) || e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_ALT_PRESSED)) combi = combi.WithAlt();
            if (e.ControlKeys.HasFlag(ControlKeyStates.LEFT_CTRL_PRESSED) || e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_CTRL_PRESSED)) combi = combi.WithCtrl();
            if (e.ControlKeys.HasFlag(ControlKeyStates.SHIFT_PRESSED)) combi = combi.WithShift();
            return combi == combination.Value;
        }
    }
}
