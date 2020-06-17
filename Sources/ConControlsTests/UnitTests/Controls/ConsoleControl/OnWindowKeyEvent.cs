/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.ConsoleApi;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void OnWindowKeyEvent_VisibleEnabledFocused_OnKeyEventCalledThreadSafe()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow, Focusable = true, Focused = true };
            stubbedWindow.KeyEventEvent(stubbedWindow, new KeyEventArgs(new ConsoleKeyEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnKeyEvent).Should().Be(1);
        }
        [TestMethod]
        public void OnWindowKeyEvent_NotVisible_OnKeyEventNotCalled()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow, Focusable = true, Focused = true, Visible = false };
            stubbedWindow.KeyEventEvent(stubbedWindow, new KeyEventArgs(new ConsoleKeyEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnKeyEvent).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowKeyEvent_NotEnabled_OnKeyEventNotCalled()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow, Focusable = true, Focused = true, Enabled = false };
            stubbedWindow.KeyEventEvent(stubbedWindow, new KeyEventArgs(new ConsoleKeyEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnKeyEvent).Should().Be(0);
        }
        [TestMethod]
        public void OnWindowKeyEvent_NotFocused_OnKeyEventNotCalled()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow, Focusable = true, Focused = false};
            stubbedWindow.KeyEventEvent(stubbedWindow, new KeyEventArgs(new ConsoleKeyEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnKeyEvent).Should().Be(0);
        }
    }
}
