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
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Focused.Should().BeFalse();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.Focused = false;
            sut.Focused.Should().BeFalse();
            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
        }
        [TestMethod]
        public void Focused_CannotFocus_InvalidOperationException()
        {
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Focused.Should().BeFalse();
            sut.CanFocus.Should().BeFalse();
            bool eventRaised = false;
            sut.FocusedChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.Invoking(s => s.Focused = true)
               .Should()
               .Throw<InvalidOperationException>();
            sut.Focused.Should().BeFalse();
            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
        }
        [TestMethod]
        public void Focused_CanFocus_SetFocus()
        {
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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

            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.Focused = true;
            sut.Focused.Should().BeTrue();
            focusedControl.Should().Be(sut);
            sut.MethodCallCounts["OnFocusedChanged"].Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Focused_CanFocus_UnsetFocus()
        {
            object syncLock = new object();
            ConControls.Controls.ConsoleControl? focusedControl = null;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                FocusedControlGet = () => focusedControl,
                FocusedControlSetConsoleControl = ctrl => focusedControl = ctrl,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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

            sut.MethodCallCounts.ContainsKey("OnFocusedChanged").Should().BeFalse();
            eventRaised.Should().BeFalse();
            sut.Focused = false;
            sut.Focused.Should().BeFalse();
            focusedControl.Should().BeNull();
            sut.MethodCallCounts["OnFocusedChanged"].Should().Be(1);
            eventRaised.Should().BeTrue();
        }
    }
}
