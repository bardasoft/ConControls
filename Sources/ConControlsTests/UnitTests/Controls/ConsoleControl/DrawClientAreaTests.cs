/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
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
        public void DrawClientArea_Disposed_ObjectDisposedException()
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
            sut.Dispose();
            sut.Invoking(s => s.DoDrawClientArea(new StubIConsoleGraphics()))
               .Should()
               .Throw<ObjectDisposedException>();
        }
        [TestMethod]
        public void DrawClientArea_GraphicsNull_ArgumentNullException()
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
            sut.Invoking(s => s.DoDrawClientArea(null!))
               .Should()
               .Throw<ArgumentNullException>()
               .Which.ParamName.Should()
               .Be("graphics");
        }
        [TestMethod]
        public void DrawClientArea_DrawingInhibited_LoggedNotDrawn()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                DrawingInhibitedGet = () => true
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            bool inhibitLogged = false;
            var sut = new TestControl(stubbedWindow);
            var child1 = new TestControl(sut);
            var child2 = new TestControl(sut);
            child1.ResetMethodCount();
            child2.ResetMethodCount();
            using var logger = new TestLogger(CheckDrawLog);
            sut.DoDrawClientArea(new StubIConsoleGraphics());
            inhibitLogged.Should().BeTrue();
            child1.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(0);
            child2.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(0);
            void CheckDrawLog(string msg)
            {
                if (msg.Contains("drawing inhibited"))
                    inhibitLogged = true;
            }
        }
        [TestMethod]
        public void DrawClientArea_ChildControlsDrawn()
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

            var sut = new TestControl(stubbedWindow)
            {
                Area = new Rectangle(1, 2, 3, 4)
            };

            var child1 = new TestControl(sut);
            var child2 = new TestControl(sut);
            child1.ResetMethodCount();
            child2.ResetMethodCount();
            sut.DoDrawClientArea(new StubIConsoleGraphics());
            child1.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(1);
            child2.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(1);
        }
    }
}
