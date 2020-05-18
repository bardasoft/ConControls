/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void FocusedBackgroundColor_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);
            sut.FocusedBackgroundColor.Should().BeNull();

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.FocusedBackgroundColor = ConsoleColor.DarkMagenta;
            sut.FocusedBackgroundColor.Should().Be(ConsoleColor.DarkMagenta);
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(1);
        }
        [TestMethod]
        public void FocusedBackgroundColor_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
            sut.FocusedBackgroundColor = sut.FocusedBackgroundColor;
            sut.FocusedBackgroundColor.Should().BeNull();
            sut.GetMethodCount(TestControl.MethodOnBackgroundColorChanged).Should().Be(0);
        }
    }
}
