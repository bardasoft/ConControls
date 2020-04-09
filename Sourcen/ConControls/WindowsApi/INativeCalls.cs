/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Drawing;
using ConControls.WindowsApi.Types;

namespace ConControls.WindowsApi
{
    interface INativeCalls
    {
        CONSOLE_SCREEN_BUFFER_INFOEX GetConsoleScreenBufferInfo(ConsoleOutputHandle consoleOutputHandle);
        string GetConsoleTitle();
        COORD GetLargestConsoleWindowSize(ConsoleOutputHandle consoleOutputHandle);
        CHAR_INFO[] ReadConsoleOutput(ConsoleOutputHandle consoleOutputHandle, Rectangle region);
        void SetConsoleMode(ConsoleInputHandle consoleInputHandle, ConsoleInputModes inputMode);
        void SetConsoleMode(ConsoleOutputHandle consoleOutputHandle, ConsoleOutputModes outputMode);
        void SetConsoleScreenBufferSize(ConsoleOutputHandle consoleOutputHandle, COORD size);
        void SetConsoleTitle(string title);
        void SetConsoleWindowSize(ConsoleOutputHandle consoleOutputHandle, Size size);
        void WriteConsoleOutput(ConsoleOutputHandle consoleOutputHandle, CHAR_INFO[] buffer, Rectangle region);
    }
}
