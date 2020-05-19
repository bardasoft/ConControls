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
            var stubbedWindow = new StubbedWindow
            {
                IsDisposedGet = () => true
            };

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Invoking(s => s.Draw())
               .Should()
               .Throw<ObjectDisposedException>()
               .Which.ObjectName.Should()
               .Be(nameof(ConsoleWindow));
        }
        [TestMethod]
        public void Draw_ControlDisposed_ObjectDisposedException()
        {
            var stubbedWindow = new StubbedWindow();
            const string name = "testname";
            var sut = new StubbedConsoleControl(stubbedWindow)
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
            var stubbedWindow = new StubbedWindow
            {
                DrawingInhibitedGet = () => true
            };
            bool inhibitLogged = false;
            var sut = new StubbedConsoleControl(stubbedWindow);
            using var logger  = new TestLogger(CheckDrawLog);
            sut.ResetMethodCount();
            sut.Draw();
            inhibitLogged.Should().BeTrue();
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBorder).Should().Be(0);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawClientArea).Should().Be(0);

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
            var stubbedWindow = new StubbedWindow
            {
                GetGraphics = () => graphics
            };
            var sut = new StubbedConsoleControl(stubbedWindow);
            stubbedWindow.GetGraphics = () => null;
            sut.ResetMethodCount();
            sut.Draw(graphics);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBorder).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawClientArea).Should().Be(1);
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
            var stubbedWindow = new StubbedWindow
            {
                GetGraphics = () => graphics
            };
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.ResetMethodCount();
            flushed = false;
            sut.Draw();
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawBorder).Should().Be(1);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawClientArea).Should().Be(1);
            flushed.Should().BeTrue();
        }
    }
}
