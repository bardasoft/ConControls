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
        public void Visible_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                VisibleGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Visible.Should().BeTrue();
            bool eventRaised = false;
            sut.VisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnVisibleChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.Visible = false;
            sut.Visible.Should().BeFalse();
            sut.MethodCallCounts["OnVisibleChanged"].Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Visible_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                VisibleGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.VisibleChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnVisibleChanged").Should().BeFalse();
            sut.Visible = sut.Visible;
            sut.Visible.Should().BeTrue();
            sut.MethodCallCounts.ContainsKey("OnVisibleChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
        }
    }
}
