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
using System.Text;
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
            using var sut = new ConControls.ConsoleApi.ConsoleListener(api);
            sut.OutputReceived += (sender, e) =>
            {
                if (e.Output == message)
                    outputReceivedSource.SetResult(0);
            };
            Assert.IsNotNull(stdoutHandle);
            using var stdoutStream = new FileStream(new SafeFileHandle(stdoutHandle!.DangerousGetHandle(), false), FileAccess.Write);
            var inbytes = Encoding.Default.GetBytes(message);
            await stdoutStream.WriteAsync(inbytes, 0, inbytes.Length);
            await stdoutStream.FlushAsync();
            (await Task.WhenAny(outputReceivedSource.Task, Task.Delay(2000)))
                .Should()
                .Be(outputReceivedSource.Task, "Message needed more than 2 seconds!");
        }
    }
}
