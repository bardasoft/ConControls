/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void EffectiveBorderStyle_NoExtraValues_DefaultValues()
        {
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                EnabledGet = () => true,
                FocusedControlGet = () => focused,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
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
            ConControls.Controls.ConsoleControl? focused = null;
            var stubbedWindow = new StubbedWindow
            {
                GetGraphics = () => new StubIConsoleGraphics(),
                FocusedControlGet = () => focused,
                EnabledGet = () => true,
                FocusedControlSetConsoleControl = c => focused = c
            };

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Focusable = true,
                DisabledBorderStyle = BorderStyle.SingleLined,
                FocusedBorderStyle = BorderStyle.Bold,
                Parent = stubbedWindow
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
