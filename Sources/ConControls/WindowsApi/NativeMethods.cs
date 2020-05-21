/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using ConControls.WindowsApi.Types;

namespace ConControls.WindowsApi
{
    [ExcludeFromCodeCoverage]
    static class NativeMethods
    {
        internal const int STDIN = -10;
        internal const int STDOUT = -11;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr CreateConsoleScreenBuffer(AccessRights desiredAccess, FileShare shareMode,
                                                                             IntPtr securityAttributes, ConsoleBufferMode bufferMode,
                                                                             IntPtr reserved);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleCursorInfo(ConsoleOutputHandle consoleOutputHandle, out CONSOLE_CURSOR_INFO cursorInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes inputMode);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetConsoleScreenBufferInfoEx(ConsoleOutputHandle consoleOutputHandle, ref CONSOLE_SCREEN_BUFFER_INFOEX info);
        [DllImport("kernel32.dll", EntryPoint = "GetConsoleTitle", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern int GetConsoleTitle(
            StringBuilder titleBuilder,
            int size);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int stdHandle);
        [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadConsoleInput(
            ConsoleInputHandle consoleInputHandle,
            [Out] INPUT_RECORD[] recordBuffer,
            int elementsInBuffer,
            out int elementsRead);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ReadConsoleOutput(
            ConsoleOutputHandle consoleOutputHandle,
            [Out] CHAR_INFO[] charInfoBuffer,
            COORD buffersize,
            COORD offset,
            ref SMALL_RECT useRegion);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleActiveScreenBuffer(ConsoleOutputHandle handle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleCursorInfo(ConsoleOutputHandle consoleOutputHandle, ref CONSOLE_CURSOR_INFO cursorInfo);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleCursorPosition(ConsoleOutputHandle consoleOutputHandle, COORD position);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleMode(ConsoleInputHandle consoleInputHandle, ConsoleInputModes inputMode);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleMode(ConsoleOutputHandle consoleOutputHandle, ConsoleOutputModes outputMode);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleTitle(string title);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleScreenBufferSize(ConsoleOutputHandle outputHandle, [In] COORD size);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetConsoleWindowInfo(ConsoleOutputHandle outputHandle, bool absolute, [In] ref SMALL_RECT rect);
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool SetStdHandle(int stdHandle, IntPtr handle);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool WriteConsoleOutput(
            ConsoleOutputHandle consoleOutputHandle,
            [In] CHAR_INFO[] charInfoBuffer,
            COORD buffersize,
            COORD offset,
            ref SMALL_RECT useRegion);
    }
}