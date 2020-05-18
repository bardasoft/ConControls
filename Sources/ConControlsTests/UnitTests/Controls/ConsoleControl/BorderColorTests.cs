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

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void BorderColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                BorderColorGet = () => ConsoleColor.Cyan
            };

            var sut = new TestControl(stubbedWindow);
            sut.BorderColor.Should().Be(ConsoleColor.Cyan);

            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.BorderColor = ConsoleColor.DarkMagenta;
            sut.BorderColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void BorderColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                BorderColorGet = () => ConsoleColor.Cyan
            };

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.BorderColor = sut.BorderColor;
            sut.BorderColor.Should().Be(ConsoleColor.Cyan);
            sut.GetMethodCount(TestControl.MethodOnBorderColorChanged).Should().Be(0);
        }
    }
}
