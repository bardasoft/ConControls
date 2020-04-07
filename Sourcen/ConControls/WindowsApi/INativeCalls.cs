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
        CHAR_INFO[] ReadConsoleOutput(ConsoleOutputHandle consoleOutputHandle, Rectangle region);
        void SetConsoleScreenBufferSize(ConsoleOutputHandle consoleOutputHandle, COORD size);
        void SetConsoleTitle(string title);
        void WriteConsoleOutput(ConsoleOutputHandle consoleOutputHandle, CHAR_INFO[] buffer, Rectangle region);
    }
}
