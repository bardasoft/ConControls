/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void Text_Null_ArgumentNullException()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedTextControl(stubbedWindow, new StubbedConsoleTextController());
            sut.Invoking(s => s.Text = null!).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void Text_NotNull_SetInControllerEventRaisedAndDrawn()
        {
            var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            var sut = new StubbedTextControl(stubbedWindow, textController) {Parent = stubbedWindow};

            const string expectedText = "expected";
            string controllerValue = string.Empty;
            textController.TextSetString = s => controllerValue = s;
            textController.TextGet = () => controllerValue;
            int eventRaised = 0;
            sut.TextChanged += (sender, e) => eventRaised += 1;

            bool drawn = false;
            stubbedWindow.Graphics.CopyCharactersConsoleColorConsoleColorPointCharArraySize = (color, consoleColor, arg3, arg4, arg5) => drawn = true;

            sut.Text = expectedText;
            drawn.Should().BeTrue();
            controllerValue.Should().Be(expectedText);
            sut.Text.Should().Be(expectedText);
            eventRaised.Should().Be(1);
            
            // don't raise event twice
            drawn = false;
            sut.Text = expectedText;
            controllerValue.Should().Be(expectedText);
            sut.Text.Should().Be(expectedText);
            eventRaised.Should().Be(1);
            drawn.Should().BeFalse();
        }
    }
}
