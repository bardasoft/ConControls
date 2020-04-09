﻿using System;
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddRange_WithWrongWindow_Exception()
        {
            var stubbedWindow = new StubIConsoleWindow();
            var differentWindow = new StubIConsoleWindow();
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);
            differentWindow.GetControls = () => new ConControls.Controls.ControlCollection(differentWindow);
            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            sut.AddRange(new ConsoleControl(stubbedWindow), new ConsoleControl(differentWindow), new ConsoleControl(stubbedWindow));
        }
        [TestMethod]
        public void AddRange_Mutliple_Distinct()
        {
            var stubbedWindow = new StubIConsoleWindow();
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new ConsoleControl(stubbedWindow);
            var control2 = new ConsoleControl(stubbedWindow);
            var control3 = new ConsoleControl(stubbedWindow);
            var control4 = new ConsoleControl(stubbedWindow);
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
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            var control1 = new ConsoleControl(stubbedWindow);
            var control2 = new ConsoleControl(stubbedWindow);
            var control3 = new ConsoleControl(stubbedWindow);
            var control4 = new ConsoleControl(stubbedWindow);

            int fired = 0;
            sut.ControlAdded += (sender, e) =>
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
            stubbedWindow.GetControls = () => new ConControls.Controls.ControlCollection(stubbedWindow);

            var sut = new ConControls.Controls.ControlCollection(stubbedWindow);
            bool fired = false;
            sut.ControlAdded += (sender, e) =>
            {
                fired = true;
                Assert.Fail();
            };
            sut.AddRange((ConsoleControl)null!);
            fired.Should().BeFalse();
        }
    }
}
