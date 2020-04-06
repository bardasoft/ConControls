using System;
using System.Drawing;

namespace ConControls
{
    /// <summary>
    /// Interface to a <see cref="ConsoleContext"/>.
    /// </summary>
    public interface IConsoleContext : IDisposable
    {
        /// <summary>
        /// Raised when the console window size changed.
        /// </summary>
        event EventHandler? SizeChanged;
        /// <summary>
        /// Raised when the background color of the console window changed.
        /// </summary>
        event EventHandler? BackgroundColorChanged;
        /// <summary>
        /// Raised when this console context is disposed of.
        /// </summary>
        event EventHandler? Disposed;

        /// <summary>
        /// Gets or sets the size of the console window.
        /// The <see cref="System.Drawing.Size.Width"/> is the number of characters per row.
        /// The <see cref="System.Drawing.Size.Height"/> is the number of lines.
        /// </summary>
        Size Size { get; set; }
        /// <summary>
        /// Gets or sets the width of the console window (in character columns).
        /// </summary>
        int Width { get; set; }
        /// <summary>
        /// Gets or sets the size of the console window (the number of rows).
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the background color of the console window.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The title of the console window.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// An object that can be used to synchronize threads
        /// that want to execute conosle operations
        /// </summary>
        object SynchronizationLock { get; }

        /// <summary>
        /// Refreshes the console window.
        /// This updates window (and buffer) sizes and colors and
        /// redraws the embedded console controls.
        /// </summary>
        void Refresh();
    }
}
