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
        public void OnMouseEvent_CalledThreadSafe()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new TestControl(stubbedWindow);
            stubbedWindow.MouseEventEvent(stubbedWindow, new MouseEventArgs(new ConsoleMouseEventArgs(default)));
            sut.GetMethodCount(TestControl.MethodOnMouseEvent).Should().Be(1);
        }
    }
}
