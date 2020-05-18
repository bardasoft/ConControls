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
        ConsoleOutputHandle CreateConsoleScreenBuffer();
        ConsoleInputModes GetConsoleMode(ConsoleInputHandle consoleInputHandle);
        ConsoleOutputModes GetConsoleMode(ConsoleOutputHandle consoleOutputHandle);
        CONSOLE_SCREEN_BUFFER_INFOEX GetConsoleScreenBufferInfo(ConsoleOutputHandle consoleOutputHandle);
        string GetConsoleTitle();
        (bool visible, int size, Point position) GetCursorInfo(ConsoleOutputHandle consoleOutputHandle);
        ConsoleInputHandle GetInputHandle();
        ConsoleOutputHandle GetOutputHandle();
        INPUT_RECORD[] ReadConsoleInput(ConsoleInputHandle consoleInputHandle, int maxElements = 1028);
        CHAR_INFO[] ReadConsoleOutput(ConsoleOutputHandle consoleOutputHandle, Rectangle region);
        bool SetActiveConsoleScreenBuffer(ConsoleOutputHandle handle);
        void SetConsoleMode(ConsoleInputHandle consoleInputHandle, ConsoleInputModes inputMode);
        void SetConsoleMode(ConsoleOutputHandle consoleOutputHandle, ConsoleOutputModes outputMode);
        void SetConsoleTitle(string title);
        void SetCursorInfo(ConsoleOutputHandle consoleOutputHandle, bool visible, int size, Point position);
        void SetInputHandle(ConsoleInputHandle inputHandle);
        void SetOutputHandle(ConsoleOutputHandle outputHandle);
        void WriteConsoleOutput(ConsoleOutputHandle consoleOutputHandle, CHAR_INFO[] buffer, Rectangle region);
    }
}
