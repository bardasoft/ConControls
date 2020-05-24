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

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void Clear_Cleared()
        {
            using var stubbedWindow = new StubbedWindow();
            bool cleared = false;
            var controller = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                MaxLineLengthGet = () => 20,
                GetLineLengthInt32 = l => 20,
                Clear = () => cleared = true
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller)
            {
                Size = (10,10).Sz(),
                Caret = (3,4).Pt(),
                Scroll = (2, 2).Pt()
            };
            sut.Size.Should().Be((10, 10).Sz());
            sut.Caret.Should().Be((3, 4).Pt());
            sut.Scroll.Should().Be((2, 2).Pt());

            sut.Clear();

            cleared.Should().BeTrue();
            sut.Caret.Should().Be(Point.Empty);
            sut.Scroll.Should().Be(Point.Empty);
        }
    }
}
