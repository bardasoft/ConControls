/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void FocusedBorderStyle_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.FocusedBorderStyle.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.FocusedBorderStyle = BorderStyle.DoubleLined;
            sut.FocusedBorderStyle.Should().Be(BorderStyle.DoubleLined);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(1);
        }
        [TestMethod]
        public void FocusedBorderStyle_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.FocusedBorderStyle = sut.FocusedBorderStyle;
            sut.FocusedBorderStyle.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
        }
    }
}
