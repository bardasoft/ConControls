/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void MouseEvents_VerticalScrollOutsideArea_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(4, 4),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollHandled_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            })) {Handled = true};
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollDisabled_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Enabled = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollInvisible_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Visible = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollTooHigh_Top()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (0, 2).Pt()
            };

            sut.Scroll.Should().Be((0,2).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = 480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollUp_ScrolledUp()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (0, 5).Pt()
            };

            sut.Scroll.Should().Be((0, 5).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = 480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((0, 1).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollDown_ScrolledDown()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (0, 5).Pt()
            };

            sut.Scroll.Should().Be((0, 5).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = -360
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((0, 8).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void MouseEvents_VerticalScrollTooLow_Bottom()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (0, 17).Pt()
            };

            sut.Scroll.Should().Be((0, 17).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.Wheeled,
                MousePosition = new COORD(5, 5),
                Scroll = -480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((0, 19).Pt());
            e.Handled.Should().BeTrue();
        }
    }
}
