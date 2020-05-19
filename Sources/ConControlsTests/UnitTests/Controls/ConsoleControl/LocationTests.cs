/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Location_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Location.Should().Be(Point.Empty);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            var point = new Point(1, 2);
            sut.Location = point;
            sut.Location.Should().Be(point);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Location_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(0);
            sut.Location = sut.Location;
            sut.Location.Should().Be(Point.Empty);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
