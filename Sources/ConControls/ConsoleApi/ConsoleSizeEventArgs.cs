/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;

namespace ConControls.ConsoleApi 
{
    sealed class ConsoleSizeEventArgs : EventArgs
    {
        public Size BufferSize { get; }
        public Rectangle WindowArea{ get; }
        public ConsoleSizeEventArgs(Rectangle windowArea, Size bufferSize)
        {
            WindowArea = windowArea;
            BufferSize = bufferSize;
        }

    }
}
