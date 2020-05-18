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
        public void RemoveRange_Removed()
        {
            using var stubbedWindow = new StubbedWindow();

            var control1 = new TestControl(stubbedWindow);
            var control2 = new TestControl(stubbedWindow);
            var control3 = new TestControl(stubbedWindow);
            var control4 = new TestControl(stubbedWindow);
            stubbedWindow.Controls.AddRange(control1, control2, control3);
            stubbedWindow.Controls.RemoveRange(control1, null!, control4);
            stubbedWindow.Controls.Should().Equal(control2, control3);
        }
        [TestMethod]
        public void RemoveRange_RemovedAndEventFired()
        {
            using var stubbedWindow = new StubbedWindow();

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new TestControl(stubbedWindow);
            var control2 = new TestControl(stubbedWindow);
            var control3 = new TestControl(stubbedWindow);
            var control4 = new TestControl(stubbedWindow);
            sut.AddRange(control1, control2, control3);

            int fired = 0;
            sut.ControlCollectionChanged += (sender, e) =>
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
            using var stubbedWindow = new StubbedWindow();

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            bool fired = false;
            sut.ControlCollectionChanged += (sender, e) =>
            {
                fired = true;
                Assert.Fail();
            };
            sut.RemoveRange(null!, new TestControl(stubbedWindow));
            fired.Should().BeFalse();
        }

    }
}
