/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddRange_WithWrongWindow_Exception()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => stubbedWindow;
            var differentWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => differentWindow;
            stubbedWindow.ControlsGet = () => new ConControls.Controls.ControlCollection(stubbedWindow);
            differentWindow.ControlsGet = () => new ConControls.Controls.ControlCollection(differentWindow);
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            sut.AddRange(new Panel(stubbedWindow), new Panel(differentWindow), new Panel(stubbedWindow));
        }
        [TestMethod]
        public void AddRange_Mutliple_Distinct()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => stubbedWindow;
            stubbedWindow.ControlsGet = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new Panel(stubbedWindow);
            var control2 = new Panel(stubbedWindow);
            var control3 = new Panel(stubbedWindow);
            var control4 = new Panel(stubbedWindow);
            sut.AddRange(control1, null!, control2);
            sut.Count.Should().Be(2);
            sut[0].Should().BeSameAs(control1);
            sut[1].Should().BeSameAs(control2);
            sut.AddRange(control3, null!, control2, control4);
            sut.Count.Should().Be(4);
            sut[0].Should().BeSameAs(control1);
            sut[1].Should().BeSameAs(control2);
            sut[2].Should().BeSameAs(control3);
            sut[3].Should().BeSameAs(control4);
        }
        [TestMethod]
        public void AddRange_Mutliple_EventFired()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.WindowGet = () => stubbedWindow;
            stubbedWindow.ControlsGet = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new Panel(stubbedWindow);
            var control2 = new Panel(stubbedWindow);
            var control3 = new Panel(stubbedWindow);
            var control4 = new Panel(stubbedWindow);

            int fired = 0;
            sut.ControlCollectionChanged += (sender, e) =>
            {
                fired++;
                if (fired == 1)
                {
                    e.AddedControls.Should().Equal(control1, control2);
                    return;
                }

                fired.Should().Be(2);
                e.AddedControls.Should().Equal(control3, control4);
            };
            sut.AddRange(control1, null!, control2);
            fired.Should().Be(1);
            sut.AddRange(control3, null!, control2, control4);
            fired.Should().Be(2);
        }
        [TestMethod]
        public void AddRange_EmptyRange_NotFired()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.ControlsGet = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            bool fired = false;
            sut.ControlCollectionChanged += (sender, e) =>
            {
                fired = true;
                Assert.Fail();
            };
            sut.AddRange((ConsoleControl)null!);
            fired.Should().BeFalse();
        }
    }
}
