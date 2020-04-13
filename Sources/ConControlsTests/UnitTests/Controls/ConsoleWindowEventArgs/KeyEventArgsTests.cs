/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleWindowEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class KeyEventArgsTests
    {
        [TestMethod]
        public void KeyEventArgs_ConstructorSetsCorrectValues()
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
            var e = new ConsoleKeyEventArgs(record);
            var sut = new KeyEventArgs(e);
            sut.KeyDown.Should().BeTrue();
            sut.ControlKeys.Should().Be(record.ControlKeys);
            sut.RepeatCount.Should().Be(123);
            sut.VirtualKey.Should().Be(record.VirtualKeyCode);
            sut.UnicodeChar.Should().Be('x');
            sut.VirtualScanCode.Should().Be(record.VirtualScanCode);
        }
    }
}
