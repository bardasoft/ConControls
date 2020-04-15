/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void CursorVisible_Changed_ThreadSafeHandlerCall()
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
            sut.CursorVisible.Should().BeFalse();
            bool eventRaised = false;
            sut.CursorVisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnCursorVisibleChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeTrue();
            sut.MethodCallCounts["OnCursorVisibleChanged"].Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void CursorVisible_NotChanged_NoEvent()
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
            sut.CursorVisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnCursorVisibleChanged").Should().BeFalse();
            sut.CursorVisible = sut.CursorVisible;
            sut.CursorVisible.Should().BeFalse();
            sut.MethodCallCounts.ContainsKey("OnCursorVisibleChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
        }
    }
}
