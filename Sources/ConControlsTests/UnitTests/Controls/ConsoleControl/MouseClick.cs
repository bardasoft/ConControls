/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Extensions;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void MouseClick_CalledThreadSafe()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow)
            {
                Area = (10, 10, 10, 10).Rect()
            };
            stubbedWindow.MouseEventEvent(stubbedWindow, new MouseEventArgs(new ConsoleMouseEventArgs(
                                                                                new MOUSE_EVENT_RECORD
                                                                                {
                                                                                    EventFlags = MouseEventFlags.Moved,
                                                                                    ButtonState = MouseButtonStates.LeftButtonPressed,
                                                                                    MousePosition = new COORD(12, 13)
                                                                                })));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseClick).Should().Be(1);
        }
        [TestMethod]
        public void MouseClick_Inconclusive()
        {
            Assert.Inconclusive();
        }
    }
}
