/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void ScrollToCaret_CaretVisible_NoChange()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Size = new Size(12, 12),
                BorderStyle = BorderStyle.SingleLined
            };

            sut.Scroll = new Point(12, 23);
            sut.Caret = new Point(15, 25);
            sut.ScrollToCaret();
            sut.Scroll.Should().BeEquivalentTo(new Point(12, 23));
        }
        [TestMethod]
        public void ScrollToCaret_CaretAbove_Changed()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Size = new Size(12, 12),
                BorderStyle = BorderStyle.SingleLined
            };

            sut.Scroll = new Point(0, 23);
            sut.Caret = new Point(5, 7);
            sut.ScrollToCaret();
            sut.Scroll.Should().BeEquivalentTo(new Point(0, 7));
        }
        [TestMethod]
        public void ScrollToCaret_CaretBelow_Changed()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Size = new Size(12, 12),
                BorderStyle = BorderStyle.SingleLined
            };

            sut.Scroll = new Point(0, 5);
            sut.Caret = new Point(5, 27);
            sut.ScrollToCaret();
            sut.Scroll.Should().BeEquivalentTo(new Point(0, 18));
        }
        [TestMethod]
        public void ScrollToCaret_CaretLeft_Changed()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Size = new Size(12, 12),
                BorderStyle = BorderStyle.SingleLined
            };

            sut.Scroll = new Point(10, 0);
            sut.Caret = new Point(5, 7);
            sut.ScrollToCaret();
            sut.Scroll.Should().BeEquivalentTo(new Point(5, 0));
        }
        [TestMethod]
        public void ScrollToCaret_CaretRight_Changed()
        {
            using var stubbedWindow = new StubbedWindow();
            var stubbedController = new StubbedConsoleTextController
            {
                ValidateCaretPoint = p => p
            };
            using var sut = new StubbedTextControl(stubbedWindow, stubbedController)
            {
                Size = new Size(12, 12),
                BorderStyle = BorderStyle.SingleLined
            };

            sut.Scroll = new Point(5, 0);
            sut.Caret = new Point(20, 7);
            sut.ScrollToCaret();
            sut.Scroll.Should().BeEquivalentTo(new Point(11, 0));
        }
    }
}
