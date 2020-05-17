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
        public void ProgressBar_ProgressCharChanged_EventRaisedOnceAndDrawn()
        {
            const char testChar = 'x';

            var window = new StubbedWindow();
            int drawn = 0;
            window.Graphics.FillAreaConsoleColorConsoleColorCharRectangle = (color, consoleColor, character, rect) =>
            {
                if (character == testChar) drawn += 1;
            };

            var sut = new ConControls.Controls.ProgressBar(window) { Size = new Size(10, 3), Orientation = ConControls.Controls.ProgressBar.ProgressOrientation.LeftToRight};
            int raised = 0;
            sut.ProgressCharChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                raised += 1;
            };

            sut.ProgressChar.Should().Be(ConControls.Controls.ProgressBar.DefaultProgressChar);

            sut.ProgressChar = testChar;
            raised.Should().Be(1);
            drawn.Should().Be(1);
            sut.ProgressChar.Should().Be(testChar);
            sut.ProgressChar = testChar;
            raised.Should().Be(1);
            drawn.Should().Be(1);
            sut.ProgressChar.Should().Be(testChar);
        }
    }
}
