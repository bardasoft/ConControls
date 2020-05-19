/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Controls_ChildAreaChanged_Handled()
        {
            var stubbedWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            var child = new StubbedConsoleControl(stubbedWindow) {Parent = sut};

            sut.ResetMethodCount();
            child.Area = new Rectangle(1, 2, 3, 4);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawClientArea).Should().Be(1);
            sut.Controls.Remove(child);
            sut.ResetMethodCount();
            child.Area = new Rectangle(2, 3, 4, 5);
            sut.GetMethodCount(StubbedConsoleControl.MethodDrawClientArea).Should().Be(0);
        }
    }
}
