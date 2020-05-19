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
        public void Parent_SameNullParent_Nothing()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().BeNull();

            sut.Parent = null;
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().BeNull();
        }
        [TestMethod]
        public void Parent_SameParent_Nothing()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(1);
            sut.Parent.Should().Be(stubbedWindow);

            sut.Parent = stubbedWindow;
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(1);
            sut.Parent.Should().Be(stubbedWindow);
        }
        [TestMethod]
        public void Parent_DifferentWindow_InvalidOperationException()
        {
            var stubbedWindow = new StubbedWindow();
            var differentWindow = new StubbedWindow();

            var sut = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            var differentParent = new StubbedConsoleControl(differentWindow) { Parent = differentWindow };
            sut.Invoking(s => s.Parent = differentParent)
               .Should()
               .Throw<InvalidOperationException>();
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(1);
            sut.Parent.Should().Be(stubbedWindow);
        }
        [TestMethod]
        public void Parent_ValidParent_ControlCollectionsChanged()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            var differentParent = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            sut.Parent = differentParent;
            sut.Parent.Should().Be(differentParent);
            sut.GetMethodCount(StubbedConsoleControl.MethodOnParentChanged).Should().Be(2);
            stubbedWindow.Controls.Should().Equal(differentParent);
            differentParent.Controls.Should().Equal(sut);
        }
    }
}
