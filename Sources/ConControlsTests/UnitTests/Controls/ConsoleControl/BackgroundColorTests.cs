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
        public void BackgroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                BackgroundColorGet = () => ConsoleColor.Cyan
            };

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.BackgroundColor.Should().Be(ConsoleColor.Cyan);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.BackgroundColor = ConsoleColor.DarkMagenta;
            sut.BackgroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBackgroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void BackgroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                BackgroundColorGet = () => ConsoleColor.Cyan
            };

            var sut = new StubbedConsoleControl(stubbedWindow);

            sut.GetMethodCount(StubbedConsoleControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.BackgroundColor = sut.BackgroundColor;
            sut.BackgroundColor.Should().Be(ConsoleColor.Cyan);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnBackgroundColorChanged).Should().Be(0);
        }
    }
}
