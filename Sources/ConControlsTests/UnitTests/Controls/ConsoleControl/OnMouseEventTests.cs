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

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void OnMouseEvent_CalledThreadSafe()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            stubbedWindow.MouseEventEvent(stubbedWindow, new MouseEventArgs(new ConsoleMouseEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnMouseEvent).Should().Be(1);
        }
    }
}
