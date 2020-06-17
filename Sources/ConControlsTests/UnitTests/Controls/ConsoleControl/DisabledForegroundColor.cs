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
        public void DisabledForegroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DisabledForegroundColor.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.DisabledForegroundColor = ConsoleColor.DarkMagenta;
            sut.DisabledForegroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void DisabledForegroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.DisabledForegroundColor = sut.DisabledForegroundColor;
            sut.DisabledForegroundColor.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
        }
    }
}
