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
        public void Focused_Unchanged_Nothing()
        {
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl
            };

            var sut = new TestControl(stubbedWindow);
            sut.Focused.Should().BeFalse();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Focused = false;
            sut.Focused.Should().BeFalse();
            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
        [TestMethod]
        public void Focused_CannotFocus_InvalidOperationException()
        {
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl
            };
            var sut = new TestControl(stubbedWindow);
            sut.Focused.Should().BeFalse();
            sut.CanFocus.Should().BeFalse();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Invoking(s => s.Focused = true)
               .Should()
               .Throw<InvalidOperationException>();
            sut.Focused.Should().BeFalse();
            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
        [TestMethod]
        public void Focused_CanFocus_SetFocus()
        {
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl
            };

            var sut = new TestControl(stubbedWindow)
            {
                Focusable = true
            };
            sut.Focused.Should().BeFalse();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Focused = true;
            sut.Focused.Should().BeTrue();
            focusedControl.Should().Be(sut);
            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Focused_CanFocus_UnsetFocus()
        {
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubbedWindow
            {
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl
            };

            var sut = new TestControl(stubbedWindow)
            {
                Focusable = true
            };
            focusedControl = sut;

            sut.Focused.Should().BeTrue();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            sut.Focused = false;
            sut.Focused.Should().BeFalse();
            focusedControl.Should().BeNull();
            sut.GetMethodCount(TestControl.MethodOnFocusedChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
    }
}
