/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

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
            internal static extern bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes inputMode);
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern IntPtr GetStdHandle(int stdHandle);
            [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool ReadConsoleInput(
                IntPtr consoleInputHandle,
                [Out] INPUT_RECORD[] recordBuffer,
                uint elementsInBuffer,
                out uint elementsRead);

        }
        public bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode) =>
            NativeMethods.GetConsoleMode(consoleInputHandle, out inputMode);
        public bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode) =>
            NativeMethods.SetConsoleMode(consoleInputHandle, inputMode);
        public bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes outputMode) =>
            NativeMethods.GetConsoleMode(consoleOutputHandle, out outputMode);
        public bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes outputMode) =>
            NativeMethods.SetConsoleMode(consoleOutputHandle, outputMode);
        public IntPtr GetStdHandle(int stdHandle) => NativeMethods.GetStdHandle(stdHandle);
        public bool ReadConsoleInput(IntPtr consoleInputHandle, INPUT_RECORD[] recordBuffer, uint elementsInBuffer, out uint elementsRead) =>
            NativeMethods.ReadConsoleInput(consoleInputHandle, recordBuffer, elementsInBuffer, out elementsRead);
    }
}
