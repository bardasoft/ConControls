/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ControlCollection
{
    public partial class ControlCollectionTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_Null_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
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
            using var stubbedWindow = new StubbedWindow();
            using var differentWindow = new StubbedWindow();
            stubbedWindow.Controls!.Add(new TestControl(differentWindow));
        }
        [TestMethod]
        public void Add_Control_Added()
        {
            using var stubbedWindow = new StubbedWindow();
            var control = new TestControl(stubbedWindow);
            stubbedWindow.Controls.Count.Should().Be(1);
            stubbedWindow.Controls[0].Should().BeSameAs(control);
        }
        [TestMethod]
        public void Add_ControlsTwice_AddedOnceAndEventsRaised()
        {
            using var stubbedWindow = new StubbedWindow();

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            ConControls.Controls.ConsoleControl? control1 = new TestControl(stubbedWindow);
            ConControls.Controls.ConsoleControl? control2 = new TestControl(stubbedWindow);
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
