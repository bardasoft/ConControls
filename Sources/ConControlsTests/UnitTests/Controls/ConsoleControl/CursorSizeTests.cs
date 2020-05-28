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
        public void CursorSize_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.CursorSize.Should().BeNull();
            bool eventRaised = false;
            sut.CursorSizeChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnCursorSizeChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.CursorSize = 23;
            sut.CursorSize.Should().Be(23);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnCursorSizeChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void CursorSize_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            bool eventRaised = false;
            sut.CursorSizeChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnCursorSizeChanged).Should().Be(0);
            sut.CursorSize = sut.CursorSize;
            sut.CursorSize.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnCursorSizeChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
