/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi.Fakes;
using ConControls.WindowsApi;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedConsoleController : StubIConsoleController
    {
        public ConsoleOutputHandle OutputHandle { get; } = new ConsoleOutputHandle(new IntPtr(23));

        internal StubbedConsoleController()
        {
            OutputHandleGet = () => OutputHandle;
        }
    }
}
