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
        public void BackgroundColor_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.BackgroundColor.Should().Be(ConsoleColor.Cyan);

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.BackgroundColor = ConsoleColor.DarkMagenta;
            sut.BackgroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void BackgroundColor_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BackgroundColorGet = () => ConsoleColor.Cyan,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.BackgroundColor = sut.BackgroundColor;
            sut.BackgroundColor.Should().Be(ConsoleColor.Cyan);
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
        }
    }
}
