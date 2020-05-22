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

                if (line.Length == 0)
                {
                    BufferLines.Add(string.Empty);
                    return;
                }

                for (int i = 0; i < line.Length; i += width)
                    BufferLines.Add(new string(line.Skip(i).Take(width).ToArray()));
                if (BufferLines[BufferLines.Count - 1].Length == width)
                    BufferLines.Add(string.Empty);
            }
        }

        readonly List<Line> lines = new List<Line>();
        readonly List<string> allLines = new List<string>();

        bool wrap;
        string text = string.Empty;
        int width;

        public int BufferLineCount => allLines.Count;
        public int MaxLineLength { get; private set; }

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
        public void Clear()
        {
            text = string.Empty;
            lines.Clear();
            allLines.Clear();
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
        public void Append(string content)
        {
            if (string.IsNullOrEmpty(content)) return;
            text += content;

            if (lines.Count > 0)
            {
                var lastLineEntry= lines[lines.Count - 1];
                lines.RemoveAt(lines.Count-1);
                allLines.RemoveRange(allLines.Count - lastLineEntry.BufferLines.Count, lastLineEntry.BufferLines.Count);
                string lastLine = string.Concat(lastLineEntry.BufferLines);
                content = lastLine + content;
            }
            
            var unwrappedLines = content.Split(new[] { "\r\n" }, StringSplitOptions.None)
                                     .SelectMany(s => s.Split(new[] { "\n" }, StringSplitOptions.None))
                                     .ToArray();
            var addedLines = unwrappedLines.Select(l => new Line(l, wrap, width)).ToArray();
            lines.AddRange(addedLines);
            allLines.AddRange(addedLines.SelectMany(l => l.BufferLines));
            MaxLineLength = allLines.Max(l => l.Length);
        }
        public Point ValidateCaret(Point caret)
        {
            int y = Math.Min(Math.Max(0, caret.Y), allLines.Count);
            if (y == allLines.Count)
                return new Point(0, allLines.Count);
            int x = Math.Min(Math.Max(0, caret.X), GetLineLength(y)-1);
            return new Point(x, y);
        }
        
        void Refresh()
        {
            string content = text;
            Clear();
            Append(content);
        }
    }
}
