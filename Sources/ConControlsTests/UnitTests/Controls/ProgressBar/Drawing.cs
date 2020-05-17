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

namespace ConControlsTests.UnitTests.Controls.ProgressBar
{
    public partial class ProgressBarTests
    {
        [TestMethod]
        public void ProgressBar_Drawing_CorrectRectangles()
        {
            const char testChar = 'T';
            var window = new StubbedWindow();
            Rectangle lastdrawnRect = default;
            window.Graphics.FillAreaConsoleColorConsoleColorCharRectangle = (color, consoleColor, character, rect) =>
            {
                if (character == testChar)
                    lastdrawnRect = rect;
            };

            var panel = new Panel(window) {Location = new Point(5, 5)};
            var sut = new ConControls.Controls.ProgressBar(panel)
            {
                Area = new Rectangle(new Point(5,5), new Size(10, 10))
            };

            sut.Percentage = 0.4;
            sut.ProgressChar = testChar;

            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.LeftToRight;
            lastdrawnRect.Should().Be(new Rectangle(10, 10, 4, 10));
            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.RightToLeft;
            lastdrawnRect.Should().Be(new Rectangle(16, 10, 4, 10));
            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.TopToBottom;
            lastdrawnRect.Should().Be(new Rectangle(10, 10, 10, 4));
            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.BottomToTop;
            lastdrawnRect.Should().Be(new Rectangle(10, 16, 10, 4));
        }
    }
}
