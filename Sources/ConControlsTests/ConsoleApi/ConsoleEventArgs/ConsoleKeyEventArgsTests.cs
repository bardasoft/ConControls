/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleKeyEventArgsTests
    {
        [TestMethod]
        public void ConsoleKeyEventArgs_ConstructorSetsCorrectValues()
        {
            KEY_EVENT_RECORD record = new KEY_EVENT_RECORD
            {
                KeyDown = 12,
                RepeatCount = 123,
                VirtualKeyCode = VirtualKey.Accept,
                UnicodeChar = 'x',
                VirtualScanCode = 321,
                ControlKeys = ControlKeyStates.RIGHT_ALT_PRESSED
            };
            var sut = new ConsoleKeyEventArgs(record);
            sut.KeyDown.Should().BeTrue();
            sut.ControlKeys.Should().Be(record.ControlKeys);
            sut.RepeatCount.Should().Be(123);
            sut.VirtualKeyCode.Should().Be(record.VirtualKeyCode);
            sut.UnicodeChar.Should().Be('x');
            sut.VirtualScanCode.Should().Be(record.VirtualScanCode);
        }
    }
}
