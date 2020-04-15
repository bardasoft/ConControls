/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using System.Linq;
using ConControls.ConsoleApi;
using ConControls.Logging;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;

namespace ConControls.Controls.Drawing 
{
    sealed class ConsoleGraphics : IConsoleGraphics
    {
        readonly INativeCalls api;
        readonly Size size;
        readonly ConsoleOutputHandle consoleOutputHandle;
        readonly CHAR_INFO[] buffer;
        readonly FrameCharSets frameCharSets;

        internal ConsoleGraphics(ConsoleOutputHandle consoleOutputHandle, INativeCalls api, Size size, FrameCharSets frameCharSets)
        {
            this.consoleOutputHandle = consoleOutputHandle;
            this.api = api;
            this.size = size;
            this.frameCharSets = frameCharSets;
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"Initializing buffer with size {this.size}.");
            buffer = api.ReadConsoleOutput(consoleOutputHandle, new Rectangle(Point.Empty, size));
        }
        public void DrawBackground(ConsoleColor color, Rectangle area)
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"drawing background {area} with {color}.");
            FillArea(color, color, default, area);
        }
        public void DrawBorder(ConsoleColor background, ConsoleColor foreground, BorderStyle style, Rectangle area)
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"drawing border {style} around {area} with {foreground} on {background}.");
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"{area.Left} {area.Top} {area.Right} {area.Bottom}");
            if (style == BorderStyle.None) return;

            var charSet = frameCharSets[style];
            var attribute = background.ToBackgroundColor() | foreground.ToForegroundColor();

            bool leftInRange = area.Left >= 0 && area.Left < size.Width;
            bool topinRange = area.Top >= 0 && area.Top < size.Height;
            bool rightInRange = area.Right > 0 && area.Right <= size.Width;
            bool bottomInRange = area.Bottom > 0 && area.Bottom <= size.Height;
            if (leftInRange)
            {
                if (topinRange)
                    buffer[GetIndex(area.Left, area.Top)] = new CHAR_INFO(charSet.UpperLeft, attribute);
                if (bottomInRange)
                    buffer[GetIndex(area.Left, area.Bottom - 1)] = new CHAR_INFO(charSet.LowerLeft, attribute);
            }
            if (rightInRange)
            {
                if (topinRange)
                    buffer[GetIndex(area.Right - 1, area.Top)] = new CHAR_INFO(charSet.UpperRight, attribute);
                if (bottomInRange)
                    buffer[GetIndex(area.Right - 1, area.Bottom - 1)] = new CHAR_INFO(charSet.LowerRight, attribute);
            }

            if (area.Width > 2)
            {
                var charInfo = new CHAR_INFO(charSet.Horizontal, attribute);
                for (int x = Math.Max(0, area.Left + 1); x < area.Right - 1 && x < size.Width; x++)
                {
                    if (topinRange)
                        buffer[GetIndex(x, area.Top)] = charInfo;
                    if (bottomInRange)
                        buffer[GetIndex(x, area.Bottom - 1)] = charInfo;
                }
            }

            if (area.Height > 2)
            {
                var charInfo = new CHAR_INFO(charSet.Vertical, attribute);
                for (int y = Math.Max(0, area.Top + 1); y < area.Bottom - 1 && y < size.Height; y++)
                {
                    if (leftInRange)
                        buffer[GetIndex(area.Left, y)] = charInfo;
                    if (rightInRange)
                        buffer[GetIndex(area.Right - 1, y)] = charInfo;
                }
            }
        }
        public void FillArea(ConsoleColor background, ConsoleColor foreColor, char c, Rectangle area)
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"Fillig area {area} with '{c}' in {foreColor} on {background}.");
            var char_info = new CHAR_INFO(c, background.ToBackgroundColor() | foreColor.ToForegroundColor());
            var indices = from x in Enumerable.Range(area.Left, area.Width)
                          from y in Enumerable.Range(area.Top, area.Height)
                          where x >= 0 && x < size.Width && y >= 0 && y < size.Height
                          select GetIndex(x, y);
            foreach (var index in indices)
                buffer[index] = char_info;
        }
        public void Flush()
        {
            Logger.Log(DebugContext.ConsoleApi | DebugContext.Graphics, $"flushing buffer ({size}).");
            api.WriteConsoleOutput(consoleOutputHandle, buffer, new Rectangle(Point.Empty, size));
        }

        int GetIndex(int x, int y) => y * size.Width + x;
    }
}
