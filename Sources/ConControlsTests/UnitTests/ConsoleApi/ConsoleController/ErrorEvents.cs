/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32.SafeHandles;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleController
{
    public partial class ConsoleControllerTests
    {
        [TestMethod]
        public async Task ErrorEvents_ReceivedCorrectEvents()
        {
            const string message = "--message--";
            TaskCompletionSource<int> errorReceivedSource = new TaskCompletionSource<int>();
            ConsoleErrorHandle? stderrHandle = null;
            using var stdin = new ManualResetEvent(false);
            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdin.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                SetErrorHandleConsoleErrorHandle = handle => stderrHandle = handle
            };
            using var sut = new ConControls.ConsoleApi.ConsoleController(Console.OutputEncoding, api);
            sut.ErrorReceived += (sender, e) =>
            {
                if (e.Output == message)
                    errorReceivedSource.SetResult(0);
            };
            Assert.IsNotNull(stderrHandle);
            using var stderrStream = new FileStream(new SafeFileHandle(stderrHandle!.DangerousGetHandle(), false), FileAccess.Write);
            var inbytes = Console.OutputEncoding.GetBytes(message);
            await stderrStream.WriteAsync(inbytes, 0, inbytes.Length);
            await stderrStream.FlushAsync();
            (await Task.WhenAny(errorReceivedSource.Task, Task.Delay(2000)))
                .Should()
                .Be(errorReceivedSource.Task, "Message needed more than 2 seconds!");
        }

        [TestMethod]
        public async Task ErrorEvents_ClosedPipe_ReadingStopped()
        {
            TaskCompletionSource<int> closeLoggedSource = new TaskCompletionSource<int>();
            ConsoleErrorHandle? stderrHandle = null;
            using var logger = new TestLogger(CheckErrorLog);
            using var stdin = new ManualResetEvent(false);
            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdin.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                SetErrorHandleConsoleErrorHandle = handle => stderrHandle = handle
            };
            using var sut = new ConControls.ConsoleApi.ConsoleController(Console.OutputEncoding, api);
            Assert.IsNotNull(stderrHandle);
            var fileHandle = new SafeFileHandle(stderrHandle!.DangerousGetHandle(), true);
            fileHandle.Close();
            (await Task.WhenAny(closeLoggedSource.Task, Task.Delay(2000)))
                .Should()
                .Be(closeLoggedSource.Task, "Closing should be done in less than 2 seconds!");
            void CheckErrorLog(string msg)
            {
                if (msg.Contains("Read zero bytes"))
                    closeLoggedSource.SetResult(0);
            }
        }
    }
}
