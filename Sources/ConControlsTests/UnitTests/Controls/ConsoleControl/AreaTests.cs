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
        public void Area_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Area.Should().Be(Rectangle.Empty);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            var rect = new Rectangle(1, 2, 3, 4);
            sut.Area = rect;
            sut.Area.Should().Be(rect);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Area_NotChanged_NoEvent()
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
            sut.Area = sut.Area;
            sut.Area.Should().Be(Rectangle.Empty);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
