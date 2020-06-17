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
        public void DisabledBorderStyle_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DisabledBorderStyle.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.DisabledBorderStyle = BorderStyle.SingleLined;
            sut.DisabledBorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(1);
        }
        [TestMethod]
        public void DisabledBorderStyle_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.DisabledBorderStyle = sut.DisabledBorderStyle;
            sut.DisabledBorderStyle.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderStyleChanged).Should().Be(0);
        }
    }
}
