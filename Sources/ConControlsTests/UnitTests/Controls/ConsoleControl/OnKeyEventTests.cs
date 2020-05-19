/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.ConsoleApi;
using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void OnKeyEvent_CalledThreadSafe()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            stubbedWindow.KeyEventEvent(stubbedWindow, new KeyEventArgs(new ConsoleKeyEventArgs(default)));
            sut.GetMethodCount(StubbedConsoleControl.MethodOnKeyEvent).Should().Be(1);
        }
    }
}
