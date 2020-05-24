/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void Text_Empty_Empty()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow);
            sut.Text.Should().BeEmpty();
        }
        [TestMethod]
        public void Text_EnclosedInParanthesis()
        {
            using var stubbedWindow = new StubbedWindow();
            string text = string.Empty;
            var stubbedController = new StubbedConsoleTextController
            {
                TextGet = () => text,
                TextSetString = s => text = s
            };
            using var sut = new ConControls.Controls.Button(stubbedWindow, stubbedController)
            {
                Text = "Hello World!"
            };
            sut.Text.Should().Be("Hello World!");
            text.Should().Be("[Hello World!]");
        }
    }
}
