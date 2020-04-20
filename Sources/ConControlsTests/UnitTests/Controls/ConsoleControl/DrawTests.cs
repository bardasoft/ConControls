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
        public void Draw_WindowDisposed_ObjectDisposedException()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics(),
                IsDisposedGet = () => true
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Invoking(s => s.Draw())
               .Should()
               .Throw<ObjectDisposedException>()
               .Which.ObjectName.Should()
               .Be(nameof(ConsoleWindow));
        }
        [TestMethod]
        public void Draw_ControlDisposed_ObjectDisposedException()
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
            const string name = "testname";
            var sut = new TestControl(stubbedWindow)
            {
                Name = name,
                Area = new Rectangle(1, 1, 10, 10)
            };
            sut.Dispose();
            sut.Invoking(s => s.Draw())
               .Should()
               .Throw<ObjectDisposedException>()
               .Which.ObjectName.Should()
               .Be(name);
        }
        [TestMethod]
        public void Draw_DrawingInhibited_LoggedNotDrawn()
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
            using var logger  = new TestLogger(CheckDrawLog);
            sut.ResetMethodCount();
            sut.Draw();
            inhibitLogged.Should().BeTrue();
            sut.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(0);
            sut.GetMethodCount(TestControl.MethodDrawBorder).Should().Be(0);
            sut.GetMethodCount(TestControl.MethodDrawClientArea).Should().Be(0);

            void CheckDrawLog(string msg)
            {
                if (msg.Contains("drawing inhibited"))
                    inhibitLogged = true;
            }
        }
        [TestMethod]
        public void Draw_WithGraphics_AllPartsDrawnNotFlushed()
        {
            bool flushed = false;
            var graphics = new StubIConsoleGraphics
            {
                Flush = () => flushed = true
            };
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
            stubbedWindow.GetGraphics = () => null;
            sut.ResetMethodCount();
            sut.Draw(graphics);
            sut.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(1);
            sut.GetMethodCount(TestControl.MethodDrawBorder).Should().Be(1);
            sut.GetMethodCount(TestControl.MethodDrawClientArea).Should().Be(1);
            flushed.Should().BeFalse();
        }
        [TestMethod]
        public void Draw_WithoutGraphics_AllPartsDrawnFlushed()
        {
            bool flushed = false;
            var graphics = new StubIConsoleGraphics
            {
                Flush = () => flushed = true
            };
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => graphics
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;
            var sut = new TestControl(stubbedWindow);
            sut.ResetMethodCount();
            flushed = false;
            sut.Draw();
            sut.GetMethodCount(TestControl.MethodDrawBackground).Should().Be(1);
            sut.GetMethodCount(TestControl.MethodDrawBorder).Should().Be(1);
            sut.GetMethodCount(TestControl.MethodDrawClientArea).Should().Be(1);
            flushed.Should().BeTrue();
        }
    }
}
