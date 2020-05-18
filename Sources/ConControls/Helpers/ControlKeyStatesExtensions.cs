/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using ConControls.WindowsApi.Types;

namespace ConControls.Helpers
{
    static class ControlKeyStatesExtensions
    {
        internal static ControlKeyStates WithoutSwitches(this ControlKeyStates controlKeys) =>
            controlKeys & ~(ControlKeyStates.CAPSLOCK_ON | ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SCROLLLOCK_ON);
    }
}
