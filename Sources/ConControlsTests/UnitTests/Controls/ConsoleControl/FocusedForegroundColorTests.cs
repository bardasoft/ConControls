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
        public void FocusedForegroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.FocusedForegroundColor.Should().BeNull();

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.FocusedForegroundColor = ConsoleColor.DarkMagenta;
            sut.FocusedForegroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void FocusedForegroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.FocusedForegroundColor = sut.FocusedForegroundColor;
            sut.FocusedForegroundColor.Should().BeNull();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
        }
    }
}
