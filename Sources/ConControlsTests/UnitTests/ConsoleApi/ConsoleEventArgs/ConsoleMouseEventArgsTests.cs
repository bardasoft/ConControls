/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleMouseEventArgsTests
    {
        [TestMethod]
        public void ConsoleMouseEventArgs_ConstructorSetsCorrectValues()
        {
            MOUSE_EVENT_RECORD record = new MOUSE_EVENT_RECORD
            {
                ControlKeys = ControlKeyStates.LEFT_ALT_PRESSED,
                EventFlags = MouseEventFlags.DoubleClick,
                ButtonState = MouseButtonStates.FourthButtonPressed,
                MousePosition = new COORD(21, 42),
                Scroll = 123
            };
            var sut = new ConsoleMouseEventArgs(record);
            sut.ControlKeys.Should().Be(record.ControlKeys);
            sut.ButtonState.Should().Be(record.ButtonState);
            sut.EventFlags.Should().Be(record.EventFlags);
            sut.MousePosition.X.Should().Be(21);
            sut.MousePosition.Y.Should().Be(42);
            sut.Scroll.Should().Be(record.Scroll);
        }
    }
}
