/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Helpers;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Helpers.KeyHandlingExtensions
{
    public partial class KeyHandlingExtensionsTests
    {
        [TestMethod]
        public void Matches_Null_False()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.LEFT_ALT_PRESSED,
                VirtualKeyCode = VirtualKey.F4
            });
            e.Matches(null).Should().BeFalse();
        }
        [TestMethod]
        public void Matches_DifferentKey_False()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.F4
            });
            var c = new KeyCombination(VirtualKey.A);
            e.Matches(c).Should().BeFalse();
        }
        [TestMethod]
        public void Matches_MissingModifier_False()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithShift();
            e.Matches(c).Should().BeFalse();
        }
        [TestMethod]
        public void Matches_TooMuchModifiers_False()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SHIFT_PRESSED | ControlKeyStates.LEFT_ALT_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithShift();
            e.Matches(c).Should().BeFalse();
        }
        [TestMethod]
        public void Matches_LeftAlt_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.LEFT_ALT_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithAlt();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_RightAlt_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.RIGHT_ALT_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithAlt();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_BothAlt_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.LEFT_ALT_PRESSED | ControlKeyStates.RIGHT_ALT_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithAlt();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_LeftCtrl_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.LEFT_CTRL_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithCtrl();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_RightCtrl_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.RIGHT_CTRL_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithCtrl();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_BothCtrl_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.LEFT_CTRL_PRESSED | ControlKeyStates.RIGHT_CTRL_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithCtrl();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_Shift_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON | ControlKeyStates.SHIFT_PRESSED,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A).WithShift();
            e.Matches(c).Should().BeTrue();
        }
        [TestMethod]
        public void Matches_NoModifiers_True()
        {
            var e = new ConsoleKeyEventArgs(new KEY_EVENT_RECORD
            {
                KeyDown = 1,
                ControlKeys = ControlKeyStates.NUMLOCK_ON,
                VirtualKeyCode = VirtualKey.A
            });
            var c = new KeyCombination(VirtualKey.A);
            e.Matches(c).Should().BeTrue();
        }
    }
}
