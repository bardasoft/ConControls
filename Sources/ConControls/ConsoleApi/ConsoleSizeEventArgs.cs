/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi 
{
    [ExcludeFromCodeCoverage]
    sealed class ConsoleSizeEventArgs : EventArgs
    {
        public Size Size { get; }
        public ConsoleSizeEventArgs(WINDOW_BUFFER_SIZE_RECORD rec)
        {
            Size = new Size(rec.Size.X, rec.Size.Y);
        }

    }
}
