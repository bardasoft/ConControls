/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void CursorPosition_Set_CaretSet()
        {
            using var stubbedWindow = new StubbedWindow();
            var controller = new StubbedConsoleTextController
            {
                BufferLineCountGet = () => 20,
                EndCaretGet = () => (20, 20).Pt(),
                GetLineLengthInt32 = l => 20,
                MaxLineLengthGet = () => 20
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller) {Parent = stubbedWindow, Size = (10, 10).Sz()};
            sut.Scroll = (4, 5).Pt();
            sut.CursorPosition = (2, 3).Pt();
            sut.Caret.Should().Be((6, 8).Pt());
        }
    }
}
