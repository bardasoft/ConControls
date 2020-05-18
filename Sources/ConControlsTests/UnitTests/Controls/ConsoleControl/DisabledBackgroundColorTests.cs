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
        public void DisabledBackgroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);
            sut.DisabledBackgroundColor.Should().BeNull();

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.DisabledBackgroundColor = ConsoleColor.DarkMagenta;
            sut.DisabledBackgroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void DisabledBackgroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.DisabledBackgroundColor = sut.DisabledBackgroundColor;
            sut.DisabledBackgroundColor.Should().BeNull();
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
        }
    }
}
