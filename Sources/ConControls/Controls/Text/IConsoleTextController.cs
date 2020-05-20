/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Drawing;

namespace ConControls.Controls.Text
{
    interface IConsoleTextController
    {
        bool Wrap { get; set; }
        int Width { get; set; }
        int BufferLineCount { get; }
        char[] GetCharacters(Rectangle area);
        int GetLineLength(int line);
        string Text { get; set; }
        void Insert(Point position, string text);
        void Remove(Point start, int length);
        Point ValidateCaret(Point caret);
    }
}
