/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Enabled_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                EnabledGet = () => true
            };

            var sut = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            sut.Enabled.Should().BeTrue();
            bool eventRaised = false;
            sut.EnabledChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnEnabledChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Enabled = false;
            sut.Enabled.Should().BeFalse();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnEnabledChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Enabled_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                EnabledGet = () => true
            };

            var sut = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            bool eventRaised = false;
            sut.EnabledChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnEnabledChanged).Should().Be(0);
            sut.Enabled = sut.Enabled;
            sut.Enabled.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnEnabledChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
