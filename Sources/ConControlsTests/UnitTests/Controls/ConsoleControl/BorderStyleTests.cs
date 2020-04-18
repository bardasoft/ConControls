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
        public void BorderStyle_Changed_ThreadSafeHandlerCall()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BorderStyleGet = () => BorderStyle.SingleLined,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.BorderStyle.Should().Be(BorderStyle.SingleLined);

            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.BorderStyle = BorderStyle.DoubleLined;
            sut.BorderStyle.Should().Be(BorderStyle.DoubleLined);
            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(1);
        }
        [TestMethod]
        public void BorderStyle_NotChanged_NoEvent()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                BorderStyleGet = () => BorderStyle.SingleLined,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.BorderStyle = sut.BorderStyle;
            sut.BorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
        }
    }
}
