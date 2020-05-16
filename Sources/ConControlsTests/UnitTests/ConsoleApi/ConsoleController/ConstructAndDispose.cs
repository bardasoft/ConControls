/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleController
{
    public partial class ConsoleControllerTests
    {
        [TestMethod]
        public void ConstructAndDispose_ModesSetCorrectly()
        {
            const ConsoleInputModes originalInputMode = ConsoleInputModes.EnableInsertMode | ConsoleInputModes.EnableAutoPosition;
            const ConsoleOutputModes originalOutputMode = ConsoleOutputModes.DisableNewLineAutoReturn;

            var inputHandle = new ConsoleInputHandle(IntPtr.Zero);
            var outputHandle = new ConsoleOutputHandle(IntPtr.Zero);

            bool inputSet = false, outputSet = false;
            bool inputReset = false, outputReset = false;

            var api = new StubINativeCalls
            {
                GetOutputHandle = () => outputHandle,
                GetInputHandle = () => inputHandle,
                GetConsoleModeConsoleInputHandle = handle =>
                {
                    inputSet.Should().BeFalse();
                    handle.Should().Be(inputHandle);
                    return originalInputMode;
                },
                GetConsoleModeConsoleOutputHandle = handle =>
                {
                    outputSet.Should().BeFalse();
                    handle.Should().Be(outputHandle);
                    return originalOutputMode;
                },
                SetConsoleModeConsoleInputHandleConsoleInputModes = (handle, mode) =>
                {
                    handle.Should().Be(inputHandle);
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
                },
                SetConsoleModeConsoleOutputHandleConsoleOutputModes = (handle, mode) =>
                {
                    handle.Should().Be(outputHandle);
                    if (!outputSet)
                    {
                        mode.Should().Be(ConsoleOutputModes.None);
                        outputSet = true;
                        return;
                    }
                    mode.Should().Be(originalOutputMode);
                    outputReset.Should().BeFalse();
                    outputReset = true;
                }
            };

            using var sut = new ConControls.ConsoleApi.ConsoleController(api);
            inputSet.Should().BeTrue();
            outputSet.Should().BeTrue();

            sut.Dispose();
            inputReset.Should().BeTrue();
            outputReset.Should().BeTrue();
        }
    }
}
