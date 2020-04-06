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
        string GetConsoleTitle();
        IntPtr GetStdHandle(int stdHandle);
        bool ReadConsoleInput(IntPtr consoleInputHandle, INPUT_RECORD[] recordBuffer, int elementsInBuffer, out int elementsRead);
        bool ReadConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset, ref SMALL_RECT useRegion);
        bool SetConsoleMode(IntPtr consoleInputHandle, ConsoleInputModes inputMode);
        bool SetConsoleMode(IntPtr consoleOutputHandle, ConsoleOutputModes outputMode);
        void SetConsoleTitle(string title);
        bool WriteConsoleOutput(IntPtr consoleOutputHandle, CHAR_INFO[] charInfoBuffer, COORD bufferSize, COORD offset, ref SMALL_RECT useRegion);
    }
}
