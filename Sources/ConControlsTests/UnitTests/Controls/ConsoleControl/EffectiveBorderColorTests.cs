/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void EffectiveBorderColor_NoExtraValues_DefaultValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.DarkYellow,
                BackgroundColorGet = () => ConsoleColor.DarkRed,
                BorderColorGet = () => ConsoleColor.Cyan,
                EnabledGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics(),
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Focusable = true
            };

            sut.EffBorderColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffBorderColor.Should().Be(ConsoleColor.Cyan);
            sut.Focused = true;
            sut.EffBorderColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = true;
            sut.EffBorderColor.Should().Be(ConsoleColor.Cyan);
        }
        [TestMethod]
        public void EffectiveBorderColor_ExtraValues_CorrectValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.DarkYellow,
                BackgroundColorGet = () => ConsoleColor.DarkRed,
                BorderColorGet = () => ConsoleColor.Cyan,
                FocusedControlGet = () => focused,
                EnabledGet = () => true,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Focusable = true,
                DisabledBorderColor = ConsoleColor.Blue,
                FocusedBorderColor = ConsoleColor.Green
            };

            sut.EffBorderColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffBorderColor.Should().Be(ConsoleColor.Blue);
            sut.Focused = true;
            sut.EffBorderColor.Should().Be(ConsoleColor.Blue);
            sut.Enabled = true;
            sut.EffBorderColor.Should().Be(ConsoleColor.Green);
        }
    }
}
