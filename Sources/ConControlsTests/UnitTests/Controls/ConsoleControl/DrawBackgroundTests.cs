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
        public void DrawBackground_Disposed_ObjectDisposedException()
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
            sut.Invoking(s => s.DoDrawBackground(new StubIConsoleGraphics()))
               .Should()
               .Throw<ObjectDisposedException>();
        }
        [TestMethod]
        public void DrawBackground_GraphicsNull_ArgumentNullException()
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
            sut.Invoking(s => s.DoDrawBackground(null!))
               .Should()
               .Throw<ArgumentNullException>()
               .Which.ParamName.Should()
               .Be("graphics");
        }
        [TestMethod]
        public void DrawBackground_DrawingInhibited_LoggedNotDrawn()
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

            bool backgroundDrawn = false;
            var graphics = new StubIConsoleGraphics
            {
                DrawBackgroundConsoleColorRectangle = (color, rect) => backgroundDrawn = true
            };
            bool inhibitLogged = false;
            var sut = new TestControl(stubbedWindow);
            using var logger = new TestLogger(CheckDrawLog);
            sut.DoDrawBackground(graphics);
            inhibitLogged.Should().BeTrue();
            backgroundDrawn.Should().BeFalse();

            void CheckDrawLog(string msg)
            {
                if (msg.Contains("drawing inhibited"))
                    inhibitLogged = true;
            }
        }
        [TestMethod]
        public void DrawBackground_BackgroundDrawn()
        {
            const ConsoleColor Color = ConsoleColor.DarkBlue;
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics(),
                BackgroundColorGet = () => Color
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow)
            {
                Area = new Rectangle(1, 2, 3, 4)
            };

            bool backgroundDrawn = false;
            var graphics = new StubIConsoleGraphics
            {
                DrawBackgroundConsoleColorRectangle = (color, rect) =>
                {
                    color.Should().Be(Color);
                    rect.Should().Be(sut.Area);
                    backgroundDrawn = true;
                }
            };
            sut.DoDrawBackground(graphics);
            backgroundDrawn.Should().BeTrue();
        }
    }
}
