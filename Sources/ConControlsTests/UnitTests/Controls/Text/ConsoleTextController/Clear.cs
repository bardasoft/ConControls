/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Text.ConsoleTextController
{
    public partial class ConsoleTextControllerTests
    {
        [TestMethod]
        public void Clear_Cleared()
        {
            const string text = "hello world!";
            var sut = new ConControls.Controls.Text.ConsoleTextController
            {
                Width = 5,
                WrapMode = WrapMode.SimpleWrap,
                Text = text
            };

            sut.Text.Should().Be(text);
            sut.Clear();
            sut.Text.Should().BeEmpty();
        }
    }
}
