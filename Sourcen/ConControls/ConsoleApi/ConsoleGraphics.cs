using System;
using System.Drawing;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi 
{
    sealed class ConsoleGraphics : IConsoleGraphics
    {
        readonly INativeCalls api;
        readonly Size size;
        readonly ConsoleOutputHandle consoleOutputHandle;
        readonly CHAR_INFO[] buffer;

        internal ConsoleGraphics(ConsoleOutputHandle consoleOutputHandle, INativeCalls api, Size size)
        {
            this.consoleOutputHandle = consoleOutputHandle;
            this.api = api;
            this.size = size;
            buffer = api.ReadConsoleOutput(consoleOutputHandle, new Rectangle(Point.Empty, size));
        }
        /// <inheritdoc />
        public void DrawBackground(ConsoleColor color, Rectangle area)
        {
            for (int x = area.Left; x < area.Right; x++)
            for (int y = area.Top; y < area.Bottom; y++)
            {
                int index = y * size.Width + x;
                buffer[index] = buffer[index].SetBackground(color).SetChar(' ');
            }
        }
        /// <inheritdoc />
        public void Flush()
        {
            api.WriteConsoleOutput(consoleOutputHandle, buffer, new Rectangle(Point.Empty, size));
        }
    }
}
