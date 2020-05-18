/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        sealed class DummyDisposable : IDisposable
        {
            public void Dispose() { }
        }
        [TestMethod]
        public void Parent_SameParent_Nothing()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new TestControl(stubbedWindow);
            sut.GetMethodCount(TestControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().Be(stubbedWindow);

            sut.Parent = stubbedWindow;
            sut.GetMethodCount(TestControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().Be(stubbedWindow);
        }
        [TestMethod]
        public void Parent_Null_ArgumentNullException()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new TestControl(stubbedWindow);
            sut.Invoking(s => s.Parent = null!)
               .Should()
               .Throw<ArgumentNullException>()
               .Which.ParamName.Should()
               .Be(nameof(sut.Parent));

            sut.GetMethodCount(TestControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().Be(stubbedWindow);
        }
        [TestMethod]
        public void Parent_DifferentWindow_InvalidOperationException()
        {
            var stubbedWindow = new StubbedWindow();
            var differentWindow = new StubbedWindow();

            var sut = new TestControl(stubbedWindow);
            var differentParent = new TestControl(differentWindow);
            sut.Invoking(s => s.Parent = differentParent)
               .Should()
               .Throw<InvalidOperationException>();
            sut.GetMethodCount(TestControl.MethodOnParentChanged).Should().Be(0);
            sut.Parent.Should().Be(stubbedWindow);
        }
        [TestMethod]
        public void Parent_ValidParent_ControlCollectionsChanged()
        {
            bool oldParentDeferred;
            bool newParentDeferred;
            var stubbedWindow = new StubbedWindow
            {
                DeferDrawing = () =>
                {
                    oldParentDeferred = true;
                    return new DummyDisposable();
                }
            };

            var sut = new TestControl(stubbedWindow);
            var differentParent = new TestControl(stubbedWindow);

            differentParent.OnDeferDrawingDisposed += () => newParentDeferred = true;

            oldParentDeferred = false;
            newParentDeferred = false;
            sut.Parent = differentParent;

            oldParentDeferred.Should().BeTrue();
            newParentDeferred.Should().BeTrue();

            sut.Parent.Should().Be(differentParent);
            sut.GetMethodCount(TestControl.MethodOnParentChanged).Should().Be(1);
            stubbedWindow.Controls.Should().Equal(differentParent);
            differentParent.Controls.Should().Equal(sut);
        }
    }
}
