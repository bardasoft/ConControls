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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConsoleControl_NullParent_ArgumentNullException()
        {
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = new StubbedConsoleControl();
        }
        [TestMethod]
        public void ConsoleControl_CompletelyInitialized_NoParentYet()
        {
            var stubbedWindow = new StubbedWindow
            {
                VisibleGet = () => true
            };

            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.Visible.Should().BeFalse();
            sut.Enabled.Should().BeFalse();
            sut.CanFocus.Should().BeFalse();
            sut.Name.Should().Be(nameof(StubbedConsoleControl));
            sut.Parent.Should().BeNull();

            sut.Parent = stubbedWindow;
            stubbedWindow.KeyEventEvent.Should().NotBeNull();
            stubbedWindow.MouseEventEvent.Should().NotBeNull();
            stubbedWindow.Controls.Should().Contain(sut);

            sut.Dispose();
            stubbedWindow.KeyEventEvent.Should().BeNull();
            stubbedWindow.MouseEventEvent.Should().BeNull();
            sut.Dispose(); // should not throw
        }
    }
}
