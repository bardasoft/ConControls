/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using ConControls.Controls;

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
        /// Draws a border around (onto the edge of) the specified <paramref name="area"/>.
        /// </summary>
        /// <param name="background">The <see cref="ConsoleColor"/> to use for the background.</param>
        /// <param name="foreground">The <see cref="ConsoleColor"/> to use for the border foreground.</param>
        /// <param name="style">The <see cref="BorderStyle"/> to use for the border.</param>
        /// <param name="area">The area (in screen buffer coordinates) to fill.</param>
        void DrawBorder(ConsoleColor background, ConsoleColor foreground, BorderStyle style, Rectangle area);
        /// <summary>
        /// Fills the specified <paramref name="area"/> with the given colors and character.
        /// </summary>
        /// <param name="background">The background color to use.</param>
        /// <param name="foreColor">The foreground color to use.</param>
        /// <param name="c">The character to use.</param>
        /// <param name="area">The area to fill.</param>
        void FillArea(ConsoleColor background, ConsoleColor foreColor, char c, Rectangle area);
        /// <summary>
        /// Flushes the internal buffer to the console screen buffer.
        /// </summary>
        void Flush();
    }
}
