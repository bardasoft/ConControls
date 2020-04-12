/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using ConControls.Controls;
using ConControls.Controls.Fakes;
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
            stubbedWindow.WindowGet = () => stubbedWindow;
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => sut;
            var control1 = new ConsolePanel(stubbedWindow);
            var control2 = new ConsolePanel(stubbedWindow);
            var control3 = new ConsolePanel(stubbedWindow);
            sut.Should().Equal(control1, control2, control3);
        }
    }
}
