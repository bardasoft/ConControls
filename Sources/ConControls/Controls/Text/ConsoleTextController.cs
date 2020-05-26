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
        int width = 1;

        public int BufferLineCount => allLines.Count;
        public int MaxLineLength { get; private set; }
        public Point EndCaret
        {
            get
            {
                int y = Math.Max(0, BufferLineCount - 1);
                int x = EndOfLine(y);
                return new Point(x, y);
            }
        }

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
                int tmp = Math.Max(1, value);
                if (width == tmp) return;
                width = tmp;
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
        internal ConsoleTextController() => Clear();
        public void Clear()
        {
            text = string.Empty;
            lines.Clear();
            allLines.Clear();
        }
        public char[] GetCharacters(Rectangle area)
        {
            area = new Rectangle(
                x: Math.Max(0, area.Left),
                y: Math.Max(0, area.Top),
                width: Math.Max(0, area.Width),
                height: Math.Max(0, area.Height));
            
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
            int y = Math.Min(Math.Max(0, caret.Y), allLines.Count-1);
            int x = Math.Max(0, NormalizeX(caret.X, y));
            return new Point(x, y);
        }
        public Point MoveCaretLeft(Point caret)
        {
            if (caret == Point.Empty) return caret;
            if (caret.X > 0) return new Point(caret.X - 1, caret.Y);
            return new Point(EndOfLine(caret.Y - 1), caret.Y - 1);
        }
        public Point MoveCaretUp(Point caret)
        {
            if (caret.Y == 0) return caret;
            return new Point(NormalizeX(caret.X, caret.Y - 1), caret.Y - 1);
        }
        public Point MoveCaretRight(Point caret)
        {
            if (caret == EndCaret) return caret;
            return ExceedsLine(caret.X + 1, caret.Y)
                ? new Point(0, caret.Y + 1)
                : new Point(caret.X + 1, caret.Y);
        }
        public Point MoveCaretDown(Point caret)
        {
            int y = caret.Y + 1;
            if (y >= BufferLineCount) return caret;
            return new Point(NormalizeX(caret.X, caret.Y + 1), caret.Y + 1);
        }
        public Point MoveCaretToBeginOfLine(Point caret) => new Point(0, caret.Y);
        public Point MoveCaretToEndOfLIne(Point caret) => new Point(EndOfLine(caret.Y), caret.Y);
        public Point MoveCaretHome(Point caret) => Point.Empty;
        public Point MoveCaretEnd(Point caret) => EndCaret;
        public Point MoveCaretPageUp(Point caret, int pageSize)
        {
            int y = Math.Max(0, caret.Y - pageSize);
            return new Point(NormalizeX(caret.X, y), y);
        }
        public Point MoveCaretPageDown(Point caret, int pageSize)
        {
            int y = Math.Max(0, Math.Min(caret.Y + pageSize, BufferLineCount - 1));
            return new Point(NormalizeX(caret.X, y), y);
        }

        void Refresh()
        {
            string content = text;
            Clear();
            Append(content);
        }
        int NormalizeX(int x, int line)
        {
            int length = GetLineLength(line);
            return Wrap && line < BufferLineCount - 1
                       ? Math.Min(x, Math.Min(length, width - 1))
                       : Math.Min(x, length);
        }
        bool ExceedsLine(int x, int y)
        {
            int length = GetLineLength(y);
            return Wrap && y < BufferLineCount - 1 && x > length - 1 || x > length;
        }
        int EndOfLine(int line)
        {
            int length = GetLineLength(line);
            return Wrap && line < BufferLineCount - 1 ? Math.Min(length, width - 1) : length;
        }
    }
}
