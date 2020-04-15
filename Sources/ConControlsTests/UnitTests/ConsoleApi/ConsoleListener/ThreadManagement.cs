/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.Threading;
using System.Threading.Tasks;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleListener
{
    public partial class ConsoleListenerTests
    {
        [TestMethod]
        public async Task ThreadManagement_ThreadStartedAndStoppedCorrectly()
        {
            TaskCompletionSource<int> startTaskSource = new TaskCompletionSource<int>();
            TaskCompletionSource<int> endTaskSource = new TaskCompletionSource<int>();
            bool threadStartLogged = false, threadEndLogged = false;

            using var stdin = new ManualResetEvent(false);
            using var logger = new TestLogger(CheckLog);
            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdin.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero)
            };
            var sut = new ConControls.ConsoleApi.ConsoleListener(api);
            (await Task.WhenAny(startTaskSource.Task, Task.Delay(2000)))
                .Should()
                .Be(startTaskSource.Task, "Thread start needed more than 2 seconds!");
            threadStartLogged.Should().BeTrue();
            sut.Dispose();
            (await Task.WhenAny(endTaskSource.Task, Task.Delay(2000)))
                .Should()
                .Be(endTaskSource.Task, "Thread stop needed more than 2 seconds!");
            threadEndLogged.Should().BeTrue();
            sut.Dispose(); // should not fail

            void CheckLog(string msg)
            {
                if (msg.Contains("Starting thread."))
                {
                    threadStartLogged = true;
                    startTaskSource.SetResult(0);
                }

                if (msg.Contains("Stopping thread"))
                {
                    threadEndLogged = true;
                    endTaskSource.SetResult(0);
                }
            }
        }
    }
}
