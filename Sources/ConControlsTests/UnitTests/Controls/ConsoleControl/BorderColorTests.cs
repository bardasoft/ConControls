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
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.BorderColor.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.BorderColor = ConsoleColor.DarkMagenta;
            sut.BorderColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void BorderColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.BorderColor = sut.BorderColor;
            sut.BorderColor.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
        }
    }
}
