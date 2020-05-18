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
        public void CursorVisible_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);
            sut.CursorVisible.Should().BeFalse();
            bool eventRaised = false;
            sut.CursorVisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnCursorVisibleChanged).Should().Be(0);
            sut.GetMethodCount(TestControl.MethodOnCursorVisibleChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            sut.GetMethodCount(TestControl.MethodOnCursorVisibleChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void CursorVisible_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.CursorVisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnCursorVisibleChanged).Should().Be(0);
            sut.CursorVisible = sut.CursorVisible;
            sut.CursorVisible.Should().BeFalse();
            sut.GetMethodCount(TestControl.MethodOnCursorVisibleChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
