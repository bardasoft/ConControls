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
        public void FocusedBackgroundColor_Changed_ThreadSafeHandlerCall()
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
            sut.FocusedBackgroundColor.Should().BeNull();

            sut.MethodCallCounts.ContainsKey("OnBackgroundColorChanged").Should().BeFalse();
            sut.FocusedBackgroundColor = ConsoleColor.DarkMagenta;
            sut.FocusedBackgroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.MethodCallCounts["OnBackgroundColorChanged"].Should().Be(1);
        }
        [TestMethod]
        public void FocusedBackgroundColor_NotChanged_NoEvent()
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

            sut.MethodCallCounts.ContainsKey("OnBackgroundColorChanged").Should().BeFalse();
            sut.FocusedBackgroundColor = sut.FocusedBackgroundColor;
            sut.FocusedBackgroundColor.Should().BeNull();
            sut.MethodCallCounts.ContainsKey("OnBackgroundColorChanged").Should().BeFalse();
        }
    }
}
