/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
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
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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
            object syncLock = new object();
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

            var differentWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics()
            };
            differentWindow.WindowGet = () => differentWindow;
            var differentCollection = new ConControls.Controls.ControlCollection(differentWindow);
            differentWindow.ControlsGet = () => differentCollection;

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
            object syncLock = new object();
            bool oldParentDeferred;
            bool newParentDeferred;
            var stubbedWindow = new StubIConsoleWindow
            {
                SynchronizationLockGet = () => syncLock,
                GetGraphics = () => new StubIConsoleGraphics(),
                DeferDrawing = () =>
                {
                    oldParentDeferred = true;
                    return new DummyDisposable();
                }
            };
            stubbedWindow.WindowGet = () => stubbedWindow;
            var controlsCollection = new ConControls.Controls.ControlCollection(stubbedWindow);
            stubbedWindow.ControlsGet = () => controlsCollection;

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
            controlsCollection.Should().Equal(differentParent);
            differentParent.Controls.Should().Equal(sut);
        }
    }
}
