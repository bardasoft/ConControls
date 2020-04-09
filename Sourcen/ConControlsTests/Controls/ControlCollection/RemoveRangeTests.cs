/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

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
        public void RemoveRange_Removed()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new ConsolePanel(stubbedWindow);
            var control2 = new ConsolePanel(stubbedWindow);
            var control3 = new ConsolePanel(stubbedWindow);
            var control4 = new ConsolePanel(stubbedWindow);
            sut.AddRange(control1, control2, control3);
            sut.RemoveRange(control1, null!, control4);
            sut.Should().Equal(control2, control3);
        }
        [TestMethod]
        public void RemoveRange_RemovedAndEventFired()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new ConsolePanel(stubbedWindow);
            var control2 = new ConsolePanel(stubbedWindow);
            var control3 = new ConsolePanel(stubbedWindow);
            var control4 = new ConsolePanel(stubbedWindow);
            sut.AddRange(control1, control2, control3);

            int fired = 0;
            sut.ControlRemoved += (sender, e) =>
            {
                fired.Should().Be(0);
                e.RemovedControls.Should().Equal(control1, control3);
                fired++;
            };
            sut.RemoveRange(control1, null!, control4, control3);
            sut.Should().Equal(control2);
            fired.Should().Be(1);
        }
        [TestMethod]
        public void RemoveRange_EmptyRange_NotFired()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            bool fired = false;
            sut.ControlRemoved += (sender, e) =>
            {
                fired = true;
                Assert.Fail();
            };
            sut.RemoveRange(null!, new ConsolePanel(stubbedWindow));
            fired.Should().BeFalse();
        }

    }
}
