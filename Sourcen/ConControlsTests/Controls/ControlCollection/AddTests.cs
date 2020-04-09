using System;
using System.Linq;
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
        public void Add_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            // ReSharper disable once AssignmentIsFullyDiscarded
            _ = new ConControls.Controls.ControlCollection(stubbedWindow)
            {
                null!
            };
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Add_WrongWindow_InvalidOperationException()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            using var differentWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var differentCollection = new ConControls.Controls.ControlCollection(differentWindow);
            differentWindow.GetControls = () => differentCollection;
            stubbedWindow.GetControls = () => sut;
            sut.Add(new ConsoleControl(differentWindow));
        }
        [TestMethod]
        public void Add_Control_Added()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.GetControls = () => sut;
            var control = new ConsoleControl(stubbedWindow);
            sut.Count.Should().Be(1);
            sut[0].Should().BeSameAs(control);
        }
        [TestMethod]
        public void Add_ControlsTwice_AddedOnceAndEventsRaised()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            // for the window, to not interfer with sut
            var stubbedCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.GetControls = () => stubbedCollection;

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            ConsoleControl? control1 = new ConsoleControl(stubbedWindow);
            ConsoleControl? control2 = new ConsoleControl(stubbedWindow);
            int added1 = 0, added2 = 0;
            sut.ControlAdded += (sender, e) =>
            {
                sender.Should().BeSameAs(sut);
                if (e.AddedControls.Contains(control1))
                    added1++;
                else if (e.AddedControls.Contains(control2))
                    added2++;
                else Assert.Fail();
            };

            sut.Add(control1);
            added1.Should().Be(1);
            added2.Should().Be(0);
            sut.Count.Should().Be(1);
            sut[0].Should().BeSameAs(control1);
            sut.Add(control2);
            added1.Should().Be(1);
            added2.Should().Be(1);
            sut.Count.Should().Be(2);
            sut[0].Should().BeSameAs(control1);
            sut[1].Should().BeSameAs(control2);
            sut.Add(control1);
            added1.Should().Be(1);
            added2.Should().Be(1);
            sut.Count.Should().Be(2);
            sut[0].Should().BeSameAs(control1);
            sut[1].Should().BeSameAs(control2);
        }
    }
}
