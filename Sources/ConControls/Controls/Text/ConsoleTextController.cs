/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ConControls.Controls.Text 
{
    sealed class ConsoleTextController : IConsoleTextController
    {
        class Line
        {
            internal List<string> BufferLines { get; } = new List<string>();

            internal Line(string line, bool wrap, int width)
            {
                if (!wrap)
                {
                    BufferLines.Add(line);
                    return;
                }

                for (int i = 0; i < line.Length; i += width)
                    BufferLines.Add(new string(line.Skip(i).Take(width).ToArray()));
            }
        }

        readonly List<Line> lines = new List<Line>();
        readonly List<string> allLines = new List<string>();

        bool wrap;
        string text = string.Empty;
        int width;

        public int BufferLineCount => allLines.Count;


        public bool Wrap
        {
            get => wrap;
            set
            {
                if (wrap == value) return;
                wrap = value;
                Refresh();
            }
        }
        /// <inheritdoc />
        public int Width
        {
            get => width;
            set
            {
                if (width == value) return;
                width = value;
                Refresh();
            }
        }
        public string Text
        {
            get => text;
            set
            {
                if (text == value) return;
                text = value;
                Refresh();
            }
        }
        public char[] GetCharacters(Rectangle area)
        {
            char[] buffer = Enumerable.Repeat('\0', area.Height * area.Width).ToArray();
            var bufferLines = allLines.Skip(area.Top).Take(area.Height);
            char[] result = bufferLines.SelectMany(MakeBufferLine).ToArray();
            Array.Copy(result, buffer, Math.Min(result.Length, buffer.Length));
            return buffer;

            IEnumerable<char> MakeBufferLine(string bl) =>
                bl.ToCharArray()
                  .Skip(area.Left)
                  .Concat(Enumerable.Repeat('\0', area.Width))
                  .Take(area.Width);
        }
        public int GetLineLength(int line) => allLines.Count > line && line >= 0 ? allLines[line].Length : 0;
        public void Append(string text)
        {
            throw new NotImplementedException();
        }
        public void AppendLine(string line)
        {
            throw new NotImplementedException();
        }
        public Point ValidateCaret(Point caret)
        {
            int y = Math.Min(Math.Max(0, caret.Y), allLines.Count);
            if (y >= allLines.Count)
                return new Point(0, allLines.Count);
            int x = Math.Min(Math.Max(0, caret.X), GetLineLength(y));
            return new Point(x, y);
        }
        
        void Refresh()
        {
            lines.Clear();
            allLines.Clear();

            var unwrappedLines = text.Split(new[] {"\r\n"}, StringSplitOptions.None)
                                     .SelectMany(s => s.Split(new[] {"\n"}, StringSplitOptions.None))
                                     .ToArray();
            lines.AddRange(unwrappedLines.Select(l => new Line(l, wrap, width)));

            allLines.AddRange(lines.SelectMany(l => l.BufferLines));
        }
    }
}
