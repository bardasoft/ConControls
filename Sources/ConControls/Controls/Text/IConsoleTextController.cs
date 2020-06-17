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
        WrapMode WrapMode { get; set; }
        int Width { get; set; }
        int BufferLineCount { get; }
        int MaxLineLength { get; }
        char[] GetCharacters(Rectangle area);
        int GetLineLength(int line);
        string Text { get; set; }
        void Clear();
        void Append(string content);
        Point EndCaret { get; }
        Point ValidateCaret(Point caret);
        Point MoveCaretLeft(Point caret);
        Point MoveCaretUp(Point caret);
        Point MoveCaretRight(Point caret);
        Point MoveCaretDown(Point caret);
        Point MoveCaretToBeginOfLine(Point caret);
        Point MoveCaretToEndOfLIne(Point caret);
        Point MoveCaretHome(Point caret);
        Point MoveCaretEnd(Point caret);
        Point MoveCaretPageUp(Point caret, int pageSize);
        Point MoveCaretPageDown(Point caret, int pageSize);
    }
}
