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
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                ForegroundColorGet = () => ConsoleColor.DarkRed,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                EnabledGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics(),
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                ForegroundColorGet = () => ConsoleColor.DarkRed,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                BorderColorGet = () => ConsoleColor.DarkYellow,
                GetGraphics = () => new StubIConsoleGraphics(),
                FocusedControlGet = () => focused,
                EnabledGet = () => true,
                FocusedControlSetConsoleControl = c => focused = c
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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
