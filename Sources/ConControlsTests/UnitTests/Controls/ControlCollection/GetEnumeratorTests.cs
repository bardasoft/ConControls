/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        public void GetEnumerator_WorksCorrectly()
        {
            using var stubbedWindow = new StubbedWindow();
            var control1 = new StubbedConsoleControl(stubbedWindow) {Parent = stubbedWindow};
            var control2 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            var control3 = new StubbedConsoleControl(stubbedWindow) { Parent = stubbedWindow };
            stubbedWindow.Controls.Should().Equal(control1, control2, control3);
        }
    }
}
