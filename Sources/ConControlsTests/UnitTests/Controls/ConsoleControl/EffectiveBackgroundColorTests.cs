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
        public void EffectiveBackgroundColor_NoExtraValues_DefaultValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.DarkRed,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                EnabledGet = () => true,
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new TestControl(stubbedWindow)
            {
                Focusable = true
            };

            sut.EffBackColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffBackColor.Should().Be(ConsoleColor.Cyan);
            sut.Focused = true;
            sut.EffBackColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = true;
            sut.EffBackColor.Should().Be(ConsoleColor.Cyan);
        }
        [TestMethod]
        public void EffectiveBackgroundColor_ExtraValues_CorrectValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.DarkRed,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                FocusedControlGet = () => focused,
                EnabledGet = () => true,
                FocusedControlSetConsoleControl = c => focused = c
            };
            var sut = new TestControl(stubbedWindow)
            {
                Focusable = true,
                DisabledBackgroundColor = ConsoleColor.Blue,
                FocusedBackgroundColor = ConsoleColor.Green
            };

            sut.EffBackColor.Should().Be(ConsoleColor.Cyan);
            sut.Enabled = false;
            sut.EffBackColor.Should().Be(ConsoleColor.Blue);
            sut.Focused = true;
            sut.EffBackColor.Should().Be(ConsoleColor.Blue);
            sut.Enabled = true;
            sut.EffBackColor.Should().Be(ConsoleColor.Green);
        }
    }
}
