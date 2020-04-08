﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using ConControls.Controls;
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
        readonly FrameCharSets frameCharSets;

        internal ConsoleGraphics(ConsoleOutputHandle consoleOutputHandle, INativeCalls api, Size size, FrameCharSets frameCharSets)
        {
            this.consoleOutputHandle = consoleOutputHandle;
            this.api = api;
            this.size = size;
            this.frameCharSets = frameCharSets;
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
                          select GetIndex(x, y);
            foreach (var index in indices)
                buffer[index] = char_info;
        }
        /// <inheritdoc />
        public void DrawBorder(ConsoleColor background, ConsoleColor foreground, BorderStyle style, Rectangle area)
        {
            Debug.WriteLine($"ConsoleGraphics.DrawBorder: drawing border {style} around {area} with {foreground} on {background}.");
            if (style == BorderStyle.None) return;

            var charSet = frameCharSets[style];
            var attribute = background.ToBackgroundColor() | foreground.ToForegroundColor();
            buffer[GetIndex(area.Left, area.Top)] = new CHAR_INFO(charSet.UpperLeft, attribute);
            buffer[GetIndex(area.Right, area.Top)] = new CHAR_INFO(charSet.UpperRight, attribute);
            buffer[GetIndex(area.Left, area.Bottom)] = new CHAR_INFO(charSet.LowerLeft, attribute);
            buffer[GetIndex(area.Right, area.Bottom)] = new CHAR_INFO(charSet.LowerRight, attribute);

            var charInfo = new CHAR_INFO(charSet.Horizontal, attribute);
            for (int x = area.Left + 1; x < area.Right; x++)
            {
                buffer[GetIndex(x, area.Top)] = charInfo;
                buffer[GetIndex(x, area.Bottom)] = charInfo;
            }
            charInfo = new CHAR_INFO(charSet.Vertical, attribute);
            for (int y = area.Top + 1; y < area.Bottom; y++)
            {
                buffer[GetIndex(area.Left, y)] = charInfo;
                buffer[GetIndex(area.Right, y)] = charInfo;
            }
        }
        /// <inheritdoc />
        public void Flush()
        {
            Debug.WriteLine($"ConsoleGraphics.Flush: flushing buffer ({size}).");
            api.WriteConsoleOutput(consoleOutputHandle, buffer, new Rectangle(Point.Empty, size));
        }

        int GetIndex(int x, int y) => y * size.Width + x;
    }
}
