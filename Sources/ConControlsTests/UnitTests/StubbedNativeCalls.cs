/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedNativeCalls : StubINativeCalls, IDisposable
    {
        public ConsoleOutputHandle StdOut { get; } = new ConsoleOutputHandle(new IntPtr(23));
        public ConsoleInputHandle StdIn { get; }
        public ManualResetEvent StdInEvent { get; } = new ManualResetEvent(false);
        public ConsoleOutputHandle ScreenHandle { get; } = new ConsoleOutputHandle(new IntPtr(42));

        internal StubbedNativeCalls()
        {
            StdIn = new ConsoleInputHandle(StdInEvent.SafeWaitHandle.DangerousGetHandle());
            GetOutputHandle = () => StdOut;
            GetInputHandle = () => StdIn;
            CreateConsoleScreenBuffer = () => ScreenHandle;
            SetActiveConsoleScreenBufferConsoleOutputHandle = handle => true;
        }
        public void Dispose()
        {
            StdInEvent.Dispose();
        }
    }
}
