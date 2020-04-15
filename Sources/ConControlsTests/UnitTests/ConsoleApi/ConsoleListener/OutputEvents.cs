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

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleListener
{
    public partial class ConsoleListenerTests
    {
        [TestMethod]
        public async Task OutputEvents_ReceivedCorrectEvents()
        {
            const string message = "--message--";
            TaskCompletionSource<int> outputReceivedSource = new TaskCompletionSource<int>();
            ConsoleOutputHandle? stdoutHandle = null;
            using var stdin = new ManualResetEvent(false);
            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdin.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                SetOutputHandleConsoleOutputHandle = handle => stdoutHandle = handle
            };
            using var sut = new ConControls.ConsoleApi.ConsoleListener(Console.OutputEncoding, api);
            sut.OutputReceived += (sender, e) =>
            {
                if (e.Output == message)
                    outputReceivedSource.SetResult(0);
            };
            Assert.IsNotNull(stdoutHandle);
            using var stdoutStream = new FileStream(new SafeFileHandle(stdoutHandle!.DangerousGetHandle(), false), FileAccess.Write);
            var inbytes = Console.OutputEncoding.GetBytes(message);
            await stdoutStream.WriteAsync(inbytes, 0, inbytes.Length);
            await stdoutStream.FlushAsync();
            (await Task.WhenAny(outputReceivedSource.Task, Task.Delay(2000)))
                .Should()
                .Be(outputReceivedSource.Task, "Message needed more than 2 seconds!");
        }

        [TestMethod]
        public async Task OutputEvents_ClosedPipe_ReadingStopped()
        {
            TaskCompletionSource<int> closeLoggedSource = new TaskCompletionSource<int>();
            ConsoleOutputHandle? stdoutHandle = null;
            using var logger = new TestLogger(CheckOutputLog);
            using var stdin = new ManualResetEvent(false);
            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdin.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => new ConsoleOutputHandle(IntPtr.Zero),
                SetOutputHandleConsoleOutputHandle = handle => stdoutHandle = handle
            };
            using var sut = new ConControls.ConsoleApi.ConsoleListener(Console.OutputEncoding, api);
            Assert.IsNotNull(stdoutHandle);
            var fileHandle = new SafeFileHandle(stdoutHandle!.DangerousGetHandle(), true);
            fileHandle.Close();
            (await Task.WhenAny(closeLoggedSource.Task, Task.Delay(5000)))
                .Should()
                .Be(closeLoggedSource.Task, "Closing should be done in less than 5 seconds!");

            void CheckOutputLog(string msg)
            {
                if (msg.Contains("Read zero bytes"))
                    closeLoggedSource.SetResult(0);
            }
        }
    }
}
