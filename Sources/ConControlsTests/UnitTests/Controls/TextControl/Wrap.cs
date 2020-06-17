/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WrapMode = ConControls.Controls.Text.WrapMode;

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void Wrap_GetControllers_Value()
        {
            using var stubbedWindow = new StubbedWindow();
            WrapMode wrap = WrapMode.NoWrap;
            var controller = new StubbedConsoleTextController
            {
                WrapModeGet = () => wrap
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller);

            sut.WrapMode.Should().Be(WrapMode.NoWrap);
            wrap = WrapMode.SimpleWrap;
            sut.WrapMode.Should().Be(WrapMode.SimpleWrap);
        }
        [TestMethod]
        public void Wrap_SetControllers_Value()
        {
            using var stubbedWindow = new StubbedWindow();
            WrapMode wrap = WrapMode.NoWrap;
            int called = 0;
            var controller = new StubbedConsoleTextController
            {
                WrapModeGet = () => wrap,
                WrapModeSetWrapMode = b =>
                {
                    called += 1;
                    wrap = b;
                }
            };
            using var sut = new StubbedTextControl(stubbedWindow, controller);

            sut.WrapMode.Should().Be(WrapMode.NoWrap);
            sut.WrapMode = WrapMode.SimpleWrap;
            called.Should().Be(1);
            wrap.Should().Be(WrapMode.SimpleWrap);
            sut.WrapMode = WrapMode.SimpleWrap;
            called.Should().Be(1);
            wrap.Should().Be(WrapMode.SimpleWrap);
            sut.WrapMode = WrapMode.NoWrap;
            called.Should().Be(2);
            wrap.Should().Be(WrapMode.NoWrap);
            sut.WrapMode = WrapMode.NoWrap;
            called.Should().Be(2);
            wrap.Should().Be(WrapMode.NoWrap);
        }
    }
}
