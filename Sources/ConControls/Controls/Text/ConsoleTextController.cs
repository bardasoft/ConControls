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
        public event EventHandler? CaretPositionChanged;
        public Size Size
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public char[] Buffer => throw new NotImplementedException();
        public string Text
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public Point CaretPosition
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }
        public void Append(string text)
        {
            throw new NotImplementedException();
        }
        public void AppendLine(string text)
        {
            throw new NotImplementedException();
        }
    }
}
