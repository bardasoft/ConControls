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
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void DrawClientArea_Disposed_ObjectDisposedException()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Dispose();
            sut.Invoking(s => s.DoDrawClientArea(new StubIConsoleGraphics()))
               .Should()
               .Throw<ObjectDisposedException>();
        }
        [TestMethod]
        public void DrawClientArea_GraphicsNull_ArgumentNullException()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Invoking(s => s.DoDrawClientArea(null!))
               .Should()
               .Throw<ArgumentNullException>()
               .Which.ParamName.Should()
               .Be("graphics");
        }
        [TestMethod]
        public void DrawClientArea_DrawingInhibited_LoggedNotDrawn()
        {
            var stubbedWindow = new StubbedWindow
            {
                DrawingInhibitedGet = () => true
            };

            bool inhibitLogged = false;
            var sut = new StubbedConsoleControl(stubbedWindow){ Parent = stubbedWindow};
            var child1 = new StubbedConsoleControl(stubbedWindow){ Parent = sut};
            var child2 = new StubbedConsoleControl(stubbedWindow){ Parent = sut};
            child1.ResetMethodCount();
            child2.ResetMethodCount();
            using var logger = new TestLogger(CheckDrawLog);
            sut.DoDrawClientArea(new StubIConsoleGraphics());
            inhibitLogged.Should().BeTrue();
            child1.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(0);
            child2.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(0);
            void CheckDrawLog(string msg)
            {
                if (msg.Contains("drawing inhibited"))
                    inhibitLogged = true;
            }
        }
        [TestMethod]
        public void DrawClientArea_ChildControlsDrawn()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = new Rectangle(1, 2, 3, 4),
                Parent = stubbedWindow
            };

            var child1 = new StubbedConsoleControl(stubbedWindow) { Parent = sut};
            var child2 = new StubbedConsoleControl(stubbedWindow){ Parent = sut};
            child1.ResetMethodCount();
            child2.ResetMethodCount();
            sut.DoDrawClientArea(new StubIConsoleGraphics());
            child1.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(1);
            child2.GetMethodCount(StubbedConsoleControl.MethodDrawBackground).Should().Be(1);
        }
    }
}
