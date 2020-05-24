/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Threading;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable PossibleNullReferenceException

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void SetActiveScreen_SetsCorrectly()
        {
            var api = new StubbedNativeCalls();
            bool active = false;
            IConsoleWindow? window = null;
            using var controller = new StubbedConsoleController
            {
                SetActiveScreenBoolean = b =>
                {
                    Assert.IsTrue(Monitor.IsEntered(window.SynchronizationLock));
                    active = b;
                }
            };

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, new StubbedGraphicsProvider());
            window = sut;
            active.Should().BeFalse();
            sut.SetActiveScreen(true);
            active.Should().BeTrue();
            sut.SetActiveScreen(false);
            active.Should().BeFalse();
        }
    }
}
