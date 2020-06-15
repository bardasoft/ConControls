/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void CanFocus_Get_True()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.Button(stubbedWindow, textController);
            sut.CanFocus.Should().BeTrue();
        }
        [TestMethod]
        public void CanFocus_Set_EventOnlyOnChange()
        {
            using var stubbedWindow = new StubbedWindow();
            var textController = new StubbedConsoleTextController();
            using var sut = new ConControls.Controls.Button(stubbedWindow, textController);
            int raised = 0;
            sut.CanFocusChanged += (sender, e) => raised++;
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(1); // base value is false before
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(1);
            sut.CanFocus = false;
            sut.CanFocus.Should().BeFalse();
            raised.Should().Be(2);
            sut.CanFocus = false;
            sut.CanFocus.Should().BeFalse();
            raised.Should().Be(2);
            sut.CanFocus = true;
            sut.CanFocus.Should().BeTrue();
            raised.Should().Be(3);
        }
    }
}
