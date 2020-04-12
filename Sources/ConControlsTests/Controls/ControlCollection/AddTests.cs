/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Linq;
using ConControls.Controls;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

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
            stubbedWindow.WindowGet = () => stubbedWindow;
            using var differentWindow = new StubIConsoleWindow();
            differentWindow.WindowGet = () => differentWindow;
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var differentCollection = new ConControls.Controls.ControlCollection(differentWindow);
            differentWindow.ControlsGet = () => differentCollection;
            stubbedWindow.ControlsGet = () => sut;
            sut.Add(new ConsolePanel(differentWindow));
        }
        [TestMethod]
        public void Add_Control_Added()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => stubbedWindow;
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => sut;
            var control = new ConsolePanel(stubbedWindow);
            sut.Count.Should().Be(1);
            sut[0].Should().BeSameAs(control);
        }
        [TestMethod]
        public void Add_ControlsTwice_AddedOnceAndEventsRaised()
        {
            using var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => stubbedWindow;
            // for the window, to not interfer with sut
            var stubbedCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => stubbedCollection;

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            ConsoleControl? control1 = new ConsolePanel(stubbedWindow);
            ConsoleControl? control2 = new ConsolePanel(stubbedWindow);
            int added1 = 0, added2 = 0;
            sut.ControlCollectionChanged += (sender, e) =>
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
