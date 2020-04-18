/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
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
        public void DrawBorder_Disposed_ObjectDisposedException()
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
            sut.Invoking(s => s.DoDrawBorder(new StubIConsoleGraphics()))
               .Should()
               .Throw<ObjectDisposedException>();
        }
        [TestMethod]
        public void DrawBorder_GraphicsNull_ArgumentNullException()
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
            sut.Invoking(s => s.DoDrawBorder(null!))
               .Should()
               .Throw<ArgumentNullException>()
               .Which.ParamName.Should()
               .Be("graphics");
        }
        [TestMethod]
        public void DrawBorder_DrawingInhibited_LoggedNotDrawn()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                DrawingInhibitedGet = () => true,
                BackgroundColorGet = () => ConsoleColor.DarkMagenta,
                BorderColorGet = () => ConsoleColor.Cyan,
                BorderStyleGet = () => BorderStyle.SingleLined
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            bool borderDrawn = false;
            var graphics = new StubIConsoleGraphics
            {
                DrawBorderConsoleColorConsoleColorBorderStyleRectangle = (bg, fg, style, rect) => borderDrawn = true
            };
            bool inhibitLogged = false;
            var sut = new TestControl(stubbedWindow);
            using var logger = new TestLogger(CheckDrawLog);
            sut.DoDrawBorder(graphics);
            inhibitLogged.Should().BeTrue();
            borderDrawn.Should().BeFalse();

            void CheckDrawLog(string msg)
            {
                if (msg.Contains("drawing inhibited"))
                    inhibitLogged = true;
            }
        }
        [TestMethod]
        public void DrawBorder_BorderDrawn()
        {
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics(),
                BackgroundColorGet = () => ConsoleColor.DarkMagenta,
                BorderColorGet = () => ConsoleColor.Cyan,
                BorderStyleGet = () => BorderStyle.None
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow)
            {
                Area = new Rectangle(1, 2, 3, 4)
            };

            bool borderDrawn = false;
            var graphics = new StubIConsoleGraphics
            {
                DrawBorderConsoleColorConsoleColorBorderStyleRectangle = (bg, fg, style, rect) =>
                {
                    fg.Should().Be(ConsoleColor.Cyan);
                    bg.Should().Be(ConsoleColor.DarkMagenta);
                    style.Should().Be(BorderStyle.None);
                    rect.Should().Be(sut.Area);
                    borderDrawn = true;
                }
            };
            sut.DoDrawBorder(graphics);
            borderDrawn.Should().BeTrue();
        }
    }
}
