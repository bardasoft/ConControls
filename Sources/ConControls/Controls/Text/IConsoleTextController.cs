/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;

namespace ConControls.Controls.Text
{
    interface IConsoleTextController
    {
        event EventHandler? BufferChanged;
        event EventHandler? CaretChanged;
        Size Size { get; set; }
        char[] Buffer { get; }
        string Text { get; set; }
        Point CaretPosition { get; set; }
        bool CaretVisible { get; set; }
        void Insert(string text);
    }
}
