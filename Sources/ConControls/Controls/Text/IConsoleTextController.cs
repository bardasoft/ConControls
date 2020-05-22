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
        int MaxLineLength { get; }
        char[] GetCharacters(Rectangle area);
        int GetLineLength(int line);
        string Text { get; set; }
        void Clear();
        void Append(string content);
        Point ValidateCaret(Point caret);
    }
}
