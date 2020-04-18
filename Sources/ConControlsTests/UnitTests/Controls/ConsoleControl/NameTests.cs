/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void Name_Changed_ThreadSafeHandlerCall()
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
            sut.Name.Should().Be(nameof(TestControl));
            bool eventRaised = false;
            sut.NameChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
            const string alt = "alt";
            sut.Name = alt;
            sut.Name.Should().Be(alt);
            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(1);
            eventRaised.Should().BeTrue();
        }
        [TestMethod]
        public void Name_NotChanged_NoEvent()
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
            sut.Name.Should().Be(nameof(TestControl));
            bool eventRaised = false;
            sut.NameChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(0);
            sut.Name = sut.Name;
            sut.Name.Should().Be(nameof(TestControl));
            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(0);
            eventRaised.Should().BeFalse();
        }
        [TestMethod]
        public void Name_Null_Default()
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

            var sut = new TestControl(stubbedWindow)
            {
                Name = "different"
            };
            bool eventRaised = false;
            sut.NameChanged += (sender, e) =>
            {
                sender.Should().Be(sut);
                eventRaised = true;
            };

            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(1);
            sut.Name = null!;
            sut.Name.Should().Be(nameof(TestControl));
            sut.GetMethodCount(TestControl.MethodOnNameChanged).Should().Be(2);
            eventRaised.Should().BeTrue();
        }
    }
}
