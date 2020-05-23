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
        public void Text_NotNull_SetInControllerAndEventRaised()
        {
            var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            var sut = new StubbedTextControl(stubbedWindow, textController);

            const string expectedText = "expected";
            string controllerValue = string.Empty;
            textController.TextSetString = s => controllerValue = s;
            textController.TextGet = () => controllerValue;
            int eventRaised = 0;
            sut.TextChanged += (sender, e) => eventRaised += 1;

            sut.Text = expectedText;
            controllerValue.Should().Be(expectedText);
            sut.Text.Should().Be(expectedText);
            eventRaised.Should().Be(1);
            
            // don't raise event twice
            sut.Text = expectedText;
            controllerValue.Should().Be(expectedText);
            sut.Text.Should().Be(expectedText);
            eventRaised.Should().Be(1);
        }
    }
}
