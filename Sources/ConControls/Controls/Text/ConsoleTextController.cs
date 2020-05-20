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
    sealed class ConsoleTextController : IConsoleTextController
    {
        public event EventHandler? BufferChanged;
        public event EventHandler? CaretChanged;
        public Size Size
        {
            get;
            set;
        }
        public char[] Buffer => new char[Size.Height * Size.Width];
        public string Text { get; set; } = string.Empty;
        public Point CaretPosition
        {
            get;
            set;
        }
        public bool CaretVisible { get; set; }
        public void Insert(string text)
        {
            throw new NotImplementedException();
        }
    }
}
