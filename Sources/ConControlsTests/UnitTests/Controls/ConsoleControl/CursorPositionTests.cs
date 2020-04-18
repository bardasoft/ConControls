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
        public void CursorPosition_Changed_ThreadSafeHandlerCall()
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
            sut.CursorPosition.Should().Be(Point.Empty);
            bool eventRaised = false;
            sut.CursorPositionChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnCursorPositionChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            var point = new Point(1, 2);
            sut.CursorPosition = point;
            sut.CursorPosition.Should().Be(point);
            sut.GetMethodCount(TestControl.MethodOnCursorPositionChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void CursorPosition_NotChanged_NoEvent()
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
            sut.CursorPositionChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnCursorPositionChanged).Should().Be(0);
            sut.CursorPosition = sut.CursorPosition;
            sut.CursorPosition.Should().Be(Point.Empty);
            sut.GetMethodCount(TestControl.MethodOnCursorPositionChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
