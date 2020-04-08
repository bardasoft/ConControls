using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
            Debug.WriteLine($"ConsoleGraphics: Initializing buffer with size {this.size}.");
            buffer = api.ReadConsoleOutput(consoleOutputHandle, new Rectangle(Point.Empty, size));
        }
        /// <inheritdoc />
        public void DrawBackground(ConsoleColor color, Rectangle area)
        {
            Debug.WriteLine($"ConsoleGraphics.DrawBackground: drawing background {area} with {color}.");
            var char_info = new CHAR_INFO(default, color.ToBackgroundColor());
            var indices = from x in Enumerable.Range(area.Left, area.Width)
                          from y in Enumerable.Range(area.Top, area.Height)
                          select y * size.Width + x;
            foreach (var index in indices)
                buffer[index] = char_info;
        }
        /// <inheritdoc />
        public void Flush()
        {
            Debug.WriteLine($"ConsoleGraphics.Flush: flushing buffer ({size}).");
            api.WriteConsoleOutput(consoleOutputHandle, buffer, new Rectangle(Point.Empty, size));
        }
    }
}
