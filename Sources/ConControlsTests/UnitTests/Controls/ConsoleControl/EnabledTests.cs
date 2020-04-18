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
        public void Enabled_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                EnabledGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Enabled.Should().BeTrue();
            bool eventRaised = false;
            sut.EnabledChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnEnabledChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Enabled = false;
            sut.Enabled.Should().BeFalse();
            sut.GetMethodCount(TestControl.MethodOnEnabledChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Enabled_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                EnabledGet = () => true,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            bool eventRaised = false;
            sut.EnabledChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnEnabledChanged).Should().Be(0);
            sut.Enabled = sut.Enabled;
            sut.Enabled.Should().BeTrue();
            sut.GetMethodCount(TestControl.MethodOnEnabledChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
    }
}
