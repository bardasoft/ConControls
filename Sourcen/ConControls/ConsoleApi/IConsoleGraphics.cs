using System;
using System.Drawing;

namespace ConControls.ConsoleApi
{
    /// <summary>
    /// Provides methods to draw on the console.
    /// </summary>
    public interface IConsoleGraphics
    {
        /// <summary>
        /// Sets the background <paramref name="color"/> in the specified <paramref name="area"/>.
        /// The changes are only written to the screen buffer when <see cref="Flush"/> is called.
        /// </summary>
        /// <param name="color">The <see cref="ConsoleColor"/> to use for the background.</param>
        /// <param name="area">The area (in screen buffer coordinates) to fill.</param>
        void DrawBackground(ConsoleColor color, Rectangle area);
        /// <summary>
        /// Flushes the internal buffer to the console screen buffer.
        /// </summary>
        void Flush();
    }
}
