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
        public void ForegroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.Cyan
            };

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.ForegroundColor.Should().Be(ConsoleColor.Cyan);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.ForegroundColor = ConsoleColor.DarkMagenta;
            sut.ForegroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void ForegroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                ForegroundColorGet = () => ConsoleColor.Cyan
            };

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
            sut.ForegroundColor = sut.ForegroundColor;
            sut.ForegroundColor.Should().Be(ConsoleColor.Cyan);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnForegroundColorChanged).Should().Be(0);
        }
    }
}
