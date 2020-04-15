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
        public void CursorSize_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                CursorSizeGet = () => 12,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.CursorSize.Should().Be(12);
            bool eventRaised = false;
            sut.CursorSizeChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnCursorSizeChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.CursorSize = 23;
            sut.CursorSize.Should().Be(23);
            sut.MethodCallCounts["OnCursorSizeChanged"].Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void CursorSize_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                CursorSizeGet = () => 12,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.CursorSizeChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnCursorSizeChanged").Should().BeFalse();
            sut.CursorSize = sut.CursorSize;
            sut.CursorSize.Should().Be(12);
            sut.MethodCallCounts.ContainsKey("OnCursorSizeChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
        }
    }
}
