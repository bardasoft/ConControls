/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.WindowsApi.Types;

namespace ConControls.WindowsApi
{
    interface INativeCalls
    {
        bool GetConsoleMode(IntPtr consoleInputHandle, out ConsoleInputModes inputMode);
        bool GetConsoleMode(IntPtr consoleOutputHandle, out ConsoleOutputModes outputMode);
        IntPtr GetStdHandle(int stdHandle);
        bool ReadConsoleInput(IntPtr consoleInputHandle, INPUT_RECORD[] recordBuffer, uint elementsInBuffer, out uint elementsRead);
        bool ReadConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset, ref SMALL_RECT useRegion);
        bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode);
        bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes outputMode);
        bool WriteConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset, ref SMALL_RECT useRegion);
    }
}
