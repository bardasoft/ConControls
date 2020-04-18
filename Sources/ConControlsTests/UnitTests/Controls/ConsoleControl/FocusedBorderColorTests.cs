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
        public void FocusedBorderColor_Changed_ThreadSafeHandlerCall()
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
            sut.FocusedBorderColor.Should().BeNull();

            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.FocusedBorderColor = ConsoleColor.DarkMagenta;
            sut.FocusedBorderColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void FocusedBorderColor_NotChanged_NoEvent()
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

            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.FocusedBorderColor = sut.FocusedBorderColor;
            sut.FocusedBorderColor.Should().BeNull();
            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
        }
    }
}
