/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleController
{
    public partial class ConsoleControllerTests
    {
        [TestMethod]
        public void SetActiveScreen_Set_OutputHandle()
        {
            var api = new StubbedNativeCalls();
            bool set = false;
            api.SetActiveConsoleScreenBufferConsoleOutputHandle = handle =>
            {
                set = handle == api.ScreenHandle;
                return true;
            };

            var sut = new ConControls.ConsoleApi.ConsoleController(api);
            sut.SetActiveScreen(true);
            set.Should().BeTrue();
        }
        [TestMethod]
        public void SetActiveScreen_Unset_OriginalHandle()
        {
            var api = new StubbedNativeCalls();
            bool set = false;
            api.SetActiveConsoleScreenBufferConsoleOutputHandle = handle =>
            {
                set = handle == api.StdOut;
                return true;
            };

            var sut = new ConControls.ConsoleApi.ConsoleController(api);
            sut.SetActiveScreen(false);
            set.Should().BeTrue();
        }
    }
}
