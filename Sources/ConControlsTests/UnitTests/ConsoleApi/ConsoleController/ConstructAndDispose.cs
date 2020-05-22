/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.ComponentModel;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleController
{
    public partial class ConsoleControllerTests
    {
        [TestMethod]
        public void Constructor_CantCreateScreenBuffer_Win32Exception()
        {
            using var api = new StubbedNativeCalls
            {
                CreateConsoleScreenBuffer = () => new ConsoleOutputHandle(IntPtr.Zero)
            };
            api.Invoking(a => new ConControls.ConsoleApi.ConsoleController(a)).Should().Throw<Win32Exception>().WithInnerException<Win32Exception>();
        }
        [TestMethod]
        public void Constructor_CantSetScreenBuffer_Win32Exception()
        {
            using var api = new StubbedNativeCalls
            {
                CreateConsoleScreenBuffer = () => new ConsoleOutputHandle(new IntPtr(42)),
                SetActiveConsoleScreenBufferConsoleOutputHandle = handle => false
            };
            api.Invoking(a => new ConControls.ConsoleApi.ConsoleController(a)).Should().Throw<Win32Exception>().WithInnerException<Win32Exception>();
        }
        [TestMethod]
        public void ConstructAndDispose_BufferAndModeSetCorrectly()
        {
            const ConsoleInputModes originalInputMode = ConsoleInputModes.EnableInsertMode | ConsoleInputModes.EnableAutoPosition;
            const string originalTitle = "original console title string";

            bool inputSet = false, outputSet = false, outputModeSet = false, titleGet = false;
            bool inputReset = false, outputReset = false, titleSet = false;

            using var api = new StubbedNativeCalls
            {
                GetConsoleTitle = () =>
                {
                    titleGet = true;
                    return originalTitle;
                },
                SetConsoleTitleString = t =>
                {
                    t.Should().Be(originalTitle);
                    titleSet = true;
                }
            };
            
            api.GetConsoleModeConsoleInputHandle = handle =>
            {
                inputSet.Should().BeFalse();
                handle.Should().Be(api.StdIn);
                return originalInputMode;
            };
            api.SetActiveConsoleScreenBufferConsoleOutputHandle = handle =>
            {
                if (!outputSet)
                {
                    handle.Should().Be(api.ScreenHandle);
                    outputReset.Should().BeFalse();
                    outputSet = true;
                    return true;
                }

                outputReset.Should().BeFalse();
                handle.Should().Be(api.StdOut);
                outputReset = true;
                return true;
            };
            api.SetConsoleModeConsoleInputHandleConsoleInputModes = (handle, mode) =>
            {
                handle.Should().Be(api.StdIn);
                if (!inputSet)
                {
                    mode.Should()
                        .Be(ConsoleInputModes.EnableWindowInput |
                            ConsoleInputModes.EnableMouseInput |
                            ConsoleInputModes.EnableExtendedFlags);
                    inputSet = true;
                    return;
                }

                mode.Should().Be(originalInputMode);
                inputReset.Should().BeFalse();
                inputReset = true;
            };
            api.SetConsoleModeConsoleOutputHandleConsoleOutputModes = (handle, mode) =>
            {
                handle.Should().Be(api.ScreenHandle);
                outputModeSet.Should().BeFalse();
                mode.Should().Be(ConsoleOutputModes.None);
                outputModeSet = true;
            };

            using var sut = new ConControls.ConsoleApi.ConsoleController(api);
            inputSet.Should().BeTrue();
            outputSet.Should().BeTrue();
            outputModeSet.Should().BeTrue();
            titleGet.Should().BeTrue();

            sut.Dispose();
            inputReset.Should().BeTrue();
            outputReset.Should().BeTrue();
            titleSet.Should().BeTrue();
        }
    }
}
