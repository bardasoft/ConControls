/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using ConControls.WindowsApi.Types;

namespace ConControls.WindowsApi
{
    [ExcludeFromCodeCoverage]
    sealed class NativeCalls : INativeCalls
    {
        internal const int STDIN = -10;
        internal const int STDOUT = -11;
        internal const int STDERR = -12;

        static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr GetStdHandle(int stdHandle);
            [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool ReadConsoleInput(
                IntPtr consoleInputHandle,
                INPUT_RECORD[] recordBuffer,
                uint elementsInBuffer,
                out uint elementsRead);
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool ReadConsoleOutput(
                IntPtr consoleOutputHandle,
                [Out] CHAR_INFO[] charInfoBuffer,
                COORD buffersize,
                COORD offset,
                ref SMALL_RECT useRegion);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes inputMode);
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool WriteConsoleOutput(
                IntPtr consoleOutputHandle,
                [In] CHAR_INFO[] charInfoBuffer,
                COORD buffersize,
                COORD offset,
                ref SMALL_RECT useRegion);
        }

        public bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode) =>
            NativeMethods.GetConsoleMode(consoleInputHandle, out inputMode);
        public bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes outputMode) =>
            NativeMethods.GetConsoleMode(consoleOutputHandle, out outputMode);
        public IntPtr GetStdHandle(int stdHandle) => NativeMethods.GetStdHandle(stdHandle);
        public bool ReadConsoleInput(IntPtr consoleInputHandle, INPUT_RECORD[] recordBuffer, uint elementsInBuffer, out uint elementsRead) =>
            NativeMethods.ReadConsoleInput(consoleInputHandle, recordBuffer, elementsInBuffer, out elementsRead);
        public bool ReadConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset,
                                      ref SMALL_RECT useRegion) =>
            NativeMethods.ReadConsoleOutput(consoleOutputHandle, charInfoBuffer, bufferSize, offset, ref useRegion);
        public bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode) =>
            NativeMethods.SetConsoleMode(consoleInputHandle, inputMode);
        public bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes outputMode) =>
            NativeMethods.SetConsoleMode(consoleOutputHandle, outputMode);
        public bool WriteConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset,
                                       ref SMALL_RECT useRegion) =>
            NativeMethods.WriteConsoleOutput(consoleOutputHandle, charInfoBuffer, bufferSize, offset, ref useRegion);
    }
}
