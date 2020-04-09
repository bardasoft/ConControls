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
        public void GetEnumerator_WorksCorrectly()
        {
            var stubbedWindow = new StubIConsoleWindow();
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.GetControls = () => sut;
            var control1 = new ConsoleControl(stubbedWindow);
            var control2 = new ConsoleControl(stubbedWindow);
            var control3 = new ConsoleControl(stubbedWindow);
            sut.Should().Equal(control1, control2, control3);
        }
    }
}
