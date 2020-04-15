/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void EffectiveBorderStyle_NoExtraValues_DefaultValues()
        {
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BorderStyleGet = () => BorderStyle.None,
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

            sut.EffBorderStyle.Should().Be(BorderStyle.None);
            sut.Enabled = false;
            sut.EffBorderStyle.Should().Be(BorderStyle.None);
            sut.Focused = true;
            sut.EffBorderStyle.Should().Be(BorderStyle.None);
            sut.Enabled = true;
            sut.EffBorderStyle.Should().Be(BorderStyle.None);
        }
        [TestMethod]
        public void EffectiveBorderStyle_ExtraValues_CorrectValues()
        {
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BorderStyleGet = () => BorderStyle.None,
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
                DisabledBorderStyle = BorderStyle.SingleLined,
                FocusedBorderStyle = BorderStyle.Bold
            };

            sut.EffBorderStyle.Should().Be(BorderStyle.None);
            sut.Enabled = false;
            sut.EffBorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.Focused = true;
            sut.EffBorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.Enabled = true;
            sut.EffBorderStyle.Should().Be(BorderStyle.Bold);
        }
    }
}
