/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void EffectiveForegroundColor_NoExtraValues_DefaultValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.Cyan,
                BackgroundColorGet = () => ConsoleColor.DarkRed,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                EnabledGet = () => true,
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Focusable = true
            };

            sut.EffForeColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffForeColor.Should().Be(ConsoleColor.Cyan);
            sut.Focused = true;
            sut.EffForeColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = true;
            sut.EffForeColor.Should().Be(ConsoleColor.Cyan);
        }
        [TestMethod]
        public void EffectiveForegroundColor_ExtraValues_CorrectValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.Cyan,
                BackgroundColorGet = () => ConsoleColor.DarkRed,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                FocusedControlGet = () => focused,
                EnabledGet = () => true,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Focusable = true,
                DisabledForegroundColor = ConsoleColor.Blue,
                FocusedForegroundColor = ConsoleColor.Green,
                Parent = stubbedWindow
            };

            sut.EffForeColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffForeColor.Should().Be(ConsoleColor.Blue);
            sut.Focused = true;
            sut.EffForeColor.Should().Be(ConsoleColor.Blue);
            sut.Enabled = true;
            sut.EffForeColor.Should().Be(ConsoleColor.Green);
        }
    }
}
