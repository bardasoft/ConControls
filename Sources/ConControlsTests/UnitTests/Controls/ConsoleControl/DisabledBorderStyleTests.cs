/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void DisabledBorderStyle_Changed_ThreadSafeHandlerCall()
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
            sut.DisabledBorderStyle.Should().BeNull();

            sut.MethodCallCounts.ContainsKey("OnBorderStyleChanged").Should().BeFalse();
            sut.DisabledBorderStyle = BorderStyle.SingleLined;
            sut.DisabledBorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.MethodCallCounts["OnBorderStyleChanged"].Should().Be(1);
        }
        [TestMethod]
        public void DisabledBorderStyle_NotChanged_NoEvent()
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

            sut.MethodCallCounts.ContainsKey("OnBorderStyleChanged").Should().BeFalse();
            sut.DisabledBorderStyle = sut.DisabledBorderStyle;
            sut.DisabledBorderStyle.Should().BeNull();
            sut.MethodCallCounts.ContainsKey("OnBorderStyleChanged").Should().BeFalse();
        }
    }
}
