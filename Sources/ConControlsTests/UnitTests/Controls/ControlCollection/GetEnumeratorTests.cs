/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
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
            var control1 = new Panel(stubbedWindow);
            var control2 = new Panel(stubbedWindow);
            var control3 = new Panel(stubbedWindow);
            sut.Should().Equal(control1, control2, control3);
        }
    }
}
