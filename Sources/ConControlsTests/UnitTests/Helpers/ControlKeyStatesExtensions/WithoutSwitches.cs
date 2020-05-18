/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Helpers;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Helpers.ControlKeyStatesExtensions
{
    public partial class ControlKeyStatesExtensionsTests
    {
        [TestMethod]
        public void WithoutSwitches_CorrectResults()
        {
            const ControlKeyStates keys = ControlKeyStates.SHIFT_PRESSED | ControlKeyStates.LEFT_ALT_PRESSED | ControlKeyStates.CAPSLOCK_ON |
                                          ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SCROLLLOCK_ON;
            keys.WithoutSwitches().Should().Be(ControlKeyStates.SHIFT_PRESSED | ControlKeyStates.LEFT_ALT_PRESSED);
        }
    }
}
