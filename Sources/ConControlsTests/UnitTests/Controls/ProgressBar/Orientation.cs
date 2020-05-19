/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ProgressBar
{
    public partial class ProgressBarTests
    {
        [TestMethod]
        public void ProgressBar_OrientationChanged_EventRaisedOnceAndDrawn()
        {
            var window = new StubbedWindow();
            int drawn = 0;
            window.Graphics.FillAreaConsoleColorConsoleColorCharRectangle = (color, consoleColor, character, rect) =>
            {
                if (character == ConControls.Controls.ProgressBar.DefaultProgressChar) drawn += 1;
            };

            var sut = new ConControls.Controls.ProgressBar(window) {Parent = window, Size = new Size(10,10)};
            int raised = 0;
            sut.OrientationChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                raised += 1;
            };

            drawn = 0;

            sut.Orientation.Should().Be(ConControls.Controls.ProgressBar.ProgressOrientation.LeftToRight);

            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.TopToBottom;
            raised.Should().Be(1);
            drawn.Should().Be(1);
            sut.Orientation.Should().Be(ConControls.Controls.ProgressBar.ProgressOrientation.TopToBottom);

            sut.Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.TopToBottom;
            raised.Should().Be(1);
            drawn.Should().Be(1);
            sut.Orientation.Should().Be(ConControls.Controls.ProgressBar.ProgressOrientation.TopToBottom);
        }
    }
}
