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
        public void Wrap_GetControllers_Value()
        {
            using var stubbedWindow = new StubbedWindow();
            bool wrap = false;
            var controller = new StubbedConsoleTextController
            {
                WrapGet = () => wrap
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller);

            sut.Wrap.Should().BeFalse();
            wrap = true;
            sut.Wrap.Should().BeTrue();
        }
        [TestMethod]
        public void Wrap_SetControllers_Value()
        {
            using var stubbedWindow = new StubbedWindow();
            bool wrap = false;
            int called = 0;
            var controller = new StubbedConsoleTextController
            {
                WrapGet = () => wrap,
                WrapSetBoolean = b =>
                {
                    called += 1;
                    wrap = b;
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller);

            sut.Wrap.Should().BeFalse();
            sut.Wrap = true;
            called.Should().Be(1);
            wrap.Should().BeTrue();
            sut.Wrap = true;
            called.Should().Be(1);
            wrap.Should().BeTrue();
            sut.Wrap = false;
            called.Should().Be(2);
            wrap.Should().BeFalse();
            sut.Wrap = false;
            called.Should().Be(2);
            wrap.Should().BeFalse();
        }
    }
}
