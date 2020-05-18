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
        public void Visible_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                VisibleGet = () => true
            };

            var sut = new TestControl(stubbedWindow);
            sut.Visible.Should().BeTrue();
            bool eventRaised = false;
            sut.VisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnVisibleChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Visible = false;
            sut.Visible.Should().BeFalse();
            sut.GetMethodCount(TestControl.MethodOnVisibleChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Visible_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                VisibleGet = () => true
            };
            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.VisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnVisibleChanged).Should().Be(0);
            sut.Visible = sut.Visible;
            sut.Visible.Should().BeTrue();
            sut.GetMethodCount(TestControl.MethodOnVisibleChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
