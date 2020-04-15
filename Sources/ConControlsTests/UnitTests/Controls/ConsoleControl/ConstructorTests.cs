/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsoleControl_NullParent_ArgumentNullException()
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = new TestControl();
        }
        [TestMethod]
        public void ConsoleControl_CompletelyInitialized_AddedToParent()
        {
            const int cursorSize = 50;
            const ConsoleColor foreground = ConsoleColor.Magenta;
            const ConsoleColor background = ConsoleColor.Cyan;
            const ConsoleColor borderColor = ConsoleColor.DarkMagenta;
            const BorderStyle borderStyle = BorderStyle.DoubleLined;
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                VisibleGet = () => true,
                CursorSizeGet = () => cursorSize,
                ForegroundColorGet = () => foreground,
                BackgroundColorGet = () => background,
                BorderColorGet = () => borderColor,
                BorderStyleGet = () => borderStyle
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var sut = new TestControl(stubbedWindow);
            sut.Visible.Should().BeTrue();
            sut.CanFocus.Should().BeFalse();
            sut.Name.Should().Be(nameof(TestControl));
            sut.CursorSize.Should().Be(cursorSize);
            sut.ForegroundColor.Should().Be(foreground);
            sut.BackgroundColor.Should().Be(background);
            sut.BorderColor.Should().Be(borderColor);
            sut.BorderStyle.Should().Be(borderStyle);
            sut.Parent.Should().Be(stubbedWindow);
            stubbedWindow.KeyEventEvent.Should().NotBeNull();
            stubbedWindow.MouseEventEvent.Should().NotBeNull();
            stubbedWindow.StdOutEventEvent.Should().NotBeNull();
            stubbedWindow.StdErrEventEvent.Should().NotBeNull();
            controlsCollection.Should().Contain(sut);

            sut.Dispose();
            stubbedWindow.KeyEventEvent.Should().BeNull();
            stubbedWindow.MouseEventEvent.Should().BeNull();
            stubbedWindow.StdOutEventEvent.Should().BeNull();
            stubbedWindow.StdErrEventEvent.Should().BeNull();
            sut.Dispose(); // should not throw
        }
    }
}
