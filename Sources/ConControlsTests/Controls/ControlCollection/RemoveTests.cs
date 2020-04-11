/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.Controls;
using ConControls.Fakes;
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
            stubbedWindow.WindowGet = () => stubbedWindow;
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => sut;
            var control1 = new ConsolePanel(stubbedWindow);
            var control2 = new ConsolePanel(stubbedWindow);
            bool called = false;
            sut.Add(control1);
            sut.Add(control2);
            sut.ControlCollectionChanged += (sender, e) =>
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
            stubbedWindow.WindowGet = () => stubbedWindow;
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => sut;
            var control3 = new ConsolePanel(stubbedWindow);
            sut.Remove(control3);
            bool called = false;
            // ReSharper disable once UnusedVariable
            var control1 = new ConsolePanel(stubbedWindow);
            // ReSharper disable once UnusedVariable
            var control2 = new ConsolePanel(stubbedWindow);
            sut.ControlCollectionChanged += (sender, e) =>
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
