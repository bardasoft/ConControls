/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ProgressBar
{
    public partial class ProgressBarTests
    {
        [TestMethod]
        public void ProgressBar_PercentageNegative_ArgumentOutOfRangeException()
        {
            var window = new StubbedWindow();
            var sut = new ConControls.Controls.ProgressBar(window);
            sut.Invoking(s => s.Percentage = -1).Should().Throw<ArgumentOutOfRangeException>();
        }
        [TestMethod]
        public void ProgressBar_PercentageGreaterThanOne_ArgumentOutOfRangeException()
        {
            var window = new StubbedWindow();
            var sut = new ConControls.Controls.ProgressBar(window);
            sut.Invoking(s => s.Percentage = 1.000000001).Should().Throw<ArgumentOutOfRangeException>();
        }
        [TestMethod]
        public void ProgressBar_PercentageChanged_EventRaisedIfChangedAndDrawnIfRectChanged()
        {
            var window = new StubbedWindow();
            int drawn = 0;
            window.Graphics.FillAreaConsoleColorConsoleColorCharRectangle = (color, consoleColor, character, rect) =>
            {
                if (character == ConControls.Controls.ProgressBar.DefaultProgressChar) drawn += 1;
            };

            var sut = new ConControls.Controls.ProgressBar(window)
            {
                Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.LeftToRight,
                Size = new Size(10, 3)
            };
            int raised = 0;
            sut.PercentageChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                raised += 1;
            };

            drawn = 0;

            sut.Percentage.Should().Be(0);
            sut.Percentage = 0.01;
            sut.Percentage.Should().Be(0.01);
            raised.Should().Be(1);
            drawn.Should().Be(1, "previous rectangles have not been stored, so this change leads to drawing.");

            sut.Percentage = 0.01;
            sut.Percentage.Should().Be(0.01);
            raised.Should().Be(1);
            drawn.Should().Be(1);

            sut.Percentage = 0.011;
            sut.Percentage.Should().Be(0.011);
            raised.Should().Be(2);
            drawn.Should().Be(1);

            sut.Percentage = 0.5;
            sut.Percentage.Should().Be(0.5);
            raised.Should().Be(3);
            drawn.Should().Be(2);

            sut.Percentage = 0.5;
            sut.Percentage.Should().Be(0.5);
            raised.Should().Be(3);
            drawn.Should().Be(2);
        }
    }
}
