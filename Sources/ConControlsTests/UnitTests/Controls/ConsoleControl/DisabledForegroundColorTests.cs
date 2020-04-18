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
        public void DisabledForegroundColor_Changed_ThreadSafeHandlerCall()
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
            sut.DisabledForegroundColor.Should().BeNull();

            sut.GetMethodCount(TestControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.DisabledForegroundColor = ConsoleColor.DarkMagenta;
            sut.DisabledForegroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnForegroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void DisabledForegroundColor_NotChanged_NoEvent()
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

            sut.GetMethodCount(TestControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.DisabledForegroundColor = sut.DisabledForegroundColor;
            sut.DisabledForegroundColor.Should().BeNull();
            sut.GetMethodCount(TestControl.MethodOnForegroundColorChanged).Should().Be(0);
        }
    }
}
