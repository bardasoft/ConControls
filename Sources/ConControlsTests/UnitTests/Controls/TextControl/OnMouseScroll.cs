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
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollOutsideArea_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(4, 4),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollHandled_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            }))
            { Handled = true };
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollDisabled_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Enabled = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollInvisible_NotScrolled()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Visible = false
            };
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = 120
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeFalse();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollTooLeft_Left()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (2, 0).Pt()
            };

            sut.Scroll.Should().Be((2, 0).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = 480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollLeft_ScrolledLeft()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (5, 0).Pt()
            };

            sut.Scroll.Should().Be((5, 0).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = 480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((1, 0).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollRight_ScrolledRight()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (5, 0).Pt()
            };

            sut.Scroll.Should().Be((5, 0).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = -360
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((8, 0).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void OnMouseScroll_HorizontalScrollTooRight_Right()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 25,
                WidthGet = () => 20,
                GetLineLengthInt32 = l => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Area = (5, 5, 10, 10).Rect(),
                Parent = stubbedWindow,
                Scroll = (22, 0).Pt()
            };

            sut.Scroll.Should().Be((22, 0).Pt());
            var e = new MouseEventArgs(new ConsoleMouseEventArgs(new MOUSE_EVENT_RECORD
            {
                EventFlags = MouseEventFlags.WheeledHorizontally,
                MousePosition = new COORD(5, 5),
                Scroll = -480
            }));
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be((24, 0).Pt());
            e.Handled.Should().BeTrue();
        }
        [TestMethod]
        public void OnMouseScroll_VerticalScrollOutsideArea_NotScrolled()
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
        public void OnMouseScroll_VerticalScrollHandled_NotScrolled()
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
            }))
            { Handled = true };
            stubbedWindow.MouseEventEvent(stubbedWindow, e);
            sut.Scroll.Should().Be(Point.Empty);
        }
        [TestMethod]
        public void OnMouseScroll_VerticalScrollDisabled_NotScrolled()
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
        public void OnMouseScroll_VerticalScrollInvisible_NotScrolled()
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
        public void OnMouseScroll_VerticalScrollTooHigh_Top()
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

            sut.Scroll.Should().Be((0, 2).Pt());
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
        public void OnMouseScroll_VerticalScrollUp_ScrolledUp()
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
        public void OnMouseScroll_VerticalScrollDown_ScrolledDown()
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
        public void OnMouseScroll_VerticalScrollTooLow_Bottom()
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
