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
        public void Size_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);
            sut.Size.Should().Be(Size.Empty);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            var size = new Size(1, 2);
            sut.Size = size;
            sut.Size.Should().Be(size);
            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Size_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            sut.Size = sut.Size;
            sut.Size.Should().Be(Size.Empty);
            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
