using System;
using ConControls.Controls;
using ConControlsTests.Stubs;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace ConControlsTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remove_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            sut.Remove(null!);
        }
        [TestMethod]
        public void Remove_Control_RemovedAndEventCalled()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.GetControls = () => sut;
            var control1 = new ConsoleControl(stubbedWindow);
            var control2 = new ConsoleControl(stubbedWindow);
            bool called = false;
            sut.Add(control1);
            sut.Add(control2);
            sut.ControlRemoved += (sender, e) =>
            {
                called.Should().BeFalse();
                called = true;
            };
            sut.Remove(control1);
            sut.Count.Should().Be(1);
            sut[0].Should().BeSameAs(control2);
            called.Should().BeTrue();
        }
        [TestMethod]
        public void Remove_ForeignControl_Nothing()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.GetControls = () => sut;
            var control3 = new ConsoleControl(stubbedWindow);
            sut.Remove(control3);
            bool called = false;
            // ReSharper disable once UnusedVariable
            var control1 = new ConsoleControl(stubbedWindow);
            // ReSharper disable once UnusedVariable
            var control2 = new ConsoleControl(stubbedWindow);
            sut.ControlRemoved += (sender, e) =>
            {
                called = true;
                Assert.Fail();
            };
            sut.Remove(control3);
            sut.Count.Should().Be(2);
            called.Should().BeFalse();
        }
    }
}
