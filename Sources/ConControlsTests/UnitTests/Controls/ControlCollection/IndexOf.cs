/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        public void IndexOf_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            stubbedWindow.Invoking(w => w.Controls.IndexOf(null!)).Should().Throw<ArgumentNullException>();
        }
        [TestMethod]
        public void IndexOf_ExternControl_MinusOne()
        {
            using var stubbedWindow = new StubbedWindow();
            using var differentWindow = new StubbedWindow();
            var control = new Panel(differentWindow);
            stubbedWindow.Controls.IndexOf(control).Should().Be(-1);
        }
        [TestMethod]
        public void IndexOf_ControlFound_Index()
        {
            var stubbedWindow = new StubbedWindow();
            var control1 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            var control2 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            var control3 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            var control4 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            stubbedWindow.Controls.IndexOf(control1).Should().Be(0);
            stubbedWindow.Controls.IndexOf(control2).Should().Be(1);
            stubbedWindow.Controls.IndexOf(control3).Should().Be(2);
            stubbedWindow.Controls.IndexOf(control4).Should().Be(3);
        }
    }
}
