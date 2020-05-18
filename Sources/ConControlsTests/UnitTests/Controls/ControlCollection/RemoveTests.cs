/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Remove_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            stubbedWindow.Controls.Remove(null!);
        }
        [TestMethod]
        public void Remove_Control_RemovedAndEventCalled()
        {
            using var stubbedWindow = new StubbedWindow();
            var control1 = new TestControl(stubbedWindow);
            var control2 = new TestControl(stubbedWindow);
            bool called = false;
            stubbedWindow.Controls.Add(control1);
            stubbedWindow.Controls.Add(control2);
            stubbedWindow.Controls.ControlCollectionChanged += (sender, e) =>
            {
                called.Should().BeFalse();
                called = true;
            };
            stubbedWindow.Controls.Remove(control1);
            stubbedWindow.Controls.Count.Should().Be(1);
            stubbedWindow.Controls[0].Should().BeSameAs(control2);
            called.Should().BeTrue();
        }
        [TestMethod]
        public void Remove_ForeignControl_Nothing()
        {
            using var stubbedWindow = new StubbedWindow();
            var control3 = new TestControl(stubbedWindow);
            stubbedWindow.Controls.Remove(control3);
            bool called = false;
            // ReSharper disable once UnusedVariable
            var control1 = new TestControl(stubbedWindow);
            // ReSharper disable once UnusedVariable
            var control2 = new TestControl(stubbedWindow);
            stubbedWindow.Controls.ControlCollectionChanged += (sender, e) =>
            {
                called = true;
                Assert.Fail();
            };
            stubbedWindow.Controls.Remove(control3);
            stubbedWindow.Controls.Count.Should().Be(2);
            called.Should().BeFalse();
        }
    }
}
