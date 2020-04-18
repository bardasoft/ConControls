/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Controls_ChildAreaChanged_Handled()
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
            var child = new TestControl(sut);

            sut.ResetMethodCount();
            child.Area = new Rectangle(1, 2, 3, 4);
            sut.GetMethodCount(TestControl.MethodDrawClientArea).Should().Be(1);
            sut.Controls.Remove(child);
            sut.ResetMethodCount();
            child.Area = new Rectangle(2, 3, 4, 5);
            sut.GetMethodCount(TestControl.MethodDrawClientArea).Should().Be(0);
        }
    }
}
