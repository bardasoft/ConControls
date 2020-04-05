/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Runtime.InteropServices;

namespace ConControls.WindowsApi
{
    interface INativeCalls
    {
        bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode);
        bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode);
        bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes outputMode);
        bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes outputMode);
        IntPtr GetStdHandle(int stdHandle);
        bool ReadConsoleInput(
            IntPtr consoleInputHandle,
            [Out] INPUT_RECORD[] recordBuffer,
            uint elementsInBuffer,
            out uint elementsRead);

    }
}
