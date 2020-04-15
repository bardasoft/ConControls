/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void DisabledBorderColor_Changed_ThreadSafeHandlerCall()
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
            sut.DisabledBorderColor.Should().BeNull();

            sut.MethodCallCounts.ContainsKey("OnBorderColorChanged").Should().BeFalse();
            sut.DisabledBorderColor = ConsoleColor.DarkMagenta;
            sut.DisabledBorderColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.MethodCallCounts["OnBorderColorChanged"].Should().Be(1);
        }
        [TestMethod]
        public void DisabledBorderColor_NotChanged_NoEvent()
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

            sut.MethodCallCounts.ContainsKey("OnBorderColorChanged").Should().BeFalse();
            sut.DisabledBorderColor = sut.DisabledBorderColor;
            sut.DisabledBorderColor.Should().BeNull();
            sut.MethodCallCounts.ContainsKey("OnBorderColorChanged").Should().BeFalse();
        }
    }
}
