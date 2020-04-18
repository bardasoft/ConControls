/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Area_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Area.Should().Be(Rectangle.Empty);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            var rect = new Rectangle(1, 2, 3, 4);
            sut.Area = rect;
            sut.Area.Should().Be(rect);
            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Area_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.AreaChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            sut.Area = sut.Area;
            sut.Area.Should().Be(Rectangle.Empty);
            sut.GetMethodCount(TestControl.MethodOnAreaChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
