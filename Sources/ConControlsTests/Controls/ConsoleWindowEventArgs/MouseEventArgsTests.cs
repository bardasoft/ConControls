/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.Controls.ConsoleWindowEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MouseEventArgsTests
    {
        [TestMethod]
        public void MouseEventArgs_ConstructorSetsCorrectValues()
        {
            MOUSE_EVENT_RECORD record = new MOUSE_EVENT_RECORD
            {
                ControlKeys = ControlKeyStates.LEFT_ALT_PRESSED,
                EventFlags = MouseEventFlags.DoubleClick,
                ButtonState = MouseButtonStates.FourthButtonPressed,
                MousePosition = new COORD(21, 42),
                Scroll = 123
            };
            var e = new ConsoleMouseEventArgs(record);
            var sut = new MouseEventArgs(e);
            sut.ControlKeys.Should().Be(record.ControlKeys);
            sut.ButtonState.Should().Be(record.ButtonState);
            sut.Kind.Should().Be(record.EventFlags);
            sut.Position.Should().Be(e.MousePosition);
            sut.Scroll.Should().Be(record.Scroll);
        }
    }
}
