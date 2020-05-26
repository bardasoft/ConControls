/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using ConControls.WindowsApi;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleController
{
    public partial class ConsoleControllerTests
    {
        [TestMethod]
        public void ActiveScreen_CorrectBehaviour()
        {
            var api = new StubbedNativeCalls();
            var sut = new ConControls.ConsoleApi.ConsoleController(api);
            sut.ActiveScreen.Should().BeTrue();

            bool set = false;
            ConsoleOutputHandle currentHandle = api.ScreenHandle;
            api.SetActiveConsoleScreenBufferConsoleOutputHandle = SetSuccess;

            sut.ActiveScreen = true;
            set.Should().BeFalse();
            sut.ActiveScreen.Should().BeTrue();
            sut.ActiveScreen = false;
            set.Should().BeTrue();
            sut.ActiveScreen.Should().BeFalse();
            currentHandle.Should().Be(api.StdOut);
            set = false;
            sut.ActiveScreen = false;
            set.Should().BeFalse();
            sut.ActiveScreen.Should().BeFalse();

            api.SetActiveConsoleScreenBufferConsoleOutputHandle = SetFail;
            sut.ActiveScreen = true;
            set.Should().BeTrue();
            sut.ActiveScreen.Should().BeFalse();

            api.SetActiveConsoleScreenBufferConsoleOutputHandle = SetSuccess;
            sut.ActiveScreen = true;
            sut.ActiveScreen.Should().BeTrue();
            currentHandle.Should().Be(api.ScreenHandle);

            set = false;
            api.SetActiveConsoleScreenBufferConsoleOutputHandle = SetFail;
            sut.ActiveScreen = false;
            set.Should().BeTrue();
            sut.ActiveScreen.Should().BeTrue();

            bool SetFail(ConsoleOutputHandle handle)
            {
                set = true;
                currentHandle = handle;
                return false;
            }
            bool SetSuccess(ConsoleOutputHandle handle)
            {
                set = true;
                currentHandle = handle;
                return true;
            }
        }
    }
}
