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
        public void DisabledBorderColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DisabledBorderColor.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.DisabledBorderColor = ConsoleColor.DarkMagenta;
            sut.DisabledBorderColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void DisabledBorderColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
            sut.DisabledBorderColor = sut.DisabledBorderColor;
            sut.DisabledBorderColor.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBorderColorChanged).Should().Be(0);
        }
    }
}
