/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

// ReSharper disable AccessToDisposedClosure

using System;
using System.Drawing;
using System.Threading;
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
        // ReSharper disable once FunctionComplexityOverflow
        public void SizeEvents_SizeChanges_EventsFired()
        {
            using var stdinEvent = new AutoResetEvent(false);
            ConsoleOutputHandle consoleOutputHandle = new ConsoleOutputHandle(IntPtr.Zero);

            object syncLock = new object();
            Rectangle windowArea = default;
            Size bufferSize = default;
            ManualResetEvent signal = new ManualResetEvent(false);

            var api = new StubINativeCalls
            {
                GetErrorHandle = () => new ConsoleErrorHandle(IntPtr.Zero),
                GetInputHandle = () => new ConsoleInputHandle(stdinEvent.SafeWaitHandle.DangerousGetHandle()),
                GetOutputHandle = () => consoleOutputHandle,
                GetConsoleScreenBufferInfoConsoleOutputHandle = handle =>
                {
                    handle.Should().Be(consoleOutputHandle);
                    return new CONSOLE_SCREEN_BUFFER_INFOEX
                    {
                        Window = new SMALL_RECT(windowArea),
                        BufferSize = new COORD(bufferSize)
                    };
                }
            };
            using var sut = new ConControls.ConsoleApi.ConsoleController(Console.OutputEncoding, api);
            sut.SizeEvent += (sender, e) =>
            {
                lock(syncLock)
                    if (e.WindowArea == windowArea &&
                        e.BufferSize == bufferSize)
                        signal.Set();
            };

            lock (syncLock)
            {
                windowArea = new Rectangle(1, 2, 3, 4);
                signal.Reset();
            }

            signal.WaitOne(2000).Should().BeTrue("size changes should be recognized within 2 seconds.");
            lock (syncLock)
            {
                bufferSize = new Size(10, 20);
                signal.Reset();
            }
            signal.WaitOne(2000).Should().BeTrue("size changes should be recognized within 2 seconds.");
        }
   }
}