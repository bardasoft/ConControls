using System;
using System.Drawing;
using ConControls.ConsoleApi;
using ConControls.Controls;

namespace ConControls
{
    /// <summary>
    /// Interface to a <see cref="ConsoleWindow"/>.
    /// </summary>
    public interface IConsoleWindow : IDisposable
    {
        /// <summary>
        /// Raised when this console window is disposed of.
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
        /// Gets the root panel of this window.
        /// </summary>
        ConsoleControl Panel { get; }

        /// <summary>
        /// Determines if the window can currently be redrawn, depending on calls to <see cref="BeginUpdate"/> and
        /// <see cref="EndUpdate"/>.
        /// </summary>
        bool DrawingInhibited { get; }
        
        /// <summary>
        /// Gets or sets the background color of the console window.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// The title of the console window.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The window has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// An object that can be used to synchronize threads
        /// that want to execute conosle operations
        /// </summary>
        object SynchronizationLock { get; }

        /// <summary>
        /// Provides a <see cref="IConsoleGraphics"/> to draw on this console window.
        /// </summary>
        /// <returns>An <see cref="IConsoleGraphics"/> iterface to draw on this window.</returns>
        IConsoleGraphics GetGraphics();
        /// <summary>
        /// Redraws the window.
        /// </summary>
        void Draw();
        /// <summary>
        /// Performs a complete refresh of the console display.
        /// </summary>
        void Refresh();
        /// <summary>
        /// Use <see cref="BeginUpdate"/> to start updating multiple properties without multiple redrawings of the window.
        /// Use <see cref="EndUpdate"/> when finished to finally redraw the window.
        /// </summary>
        void BeginUpdate();
        /// <summary>
        /// Use <see cref="BeginUpdate"/> to start updating multiple properties without multiple redrawings of the window.
        /// Use <see cref="EndUpdate"/> when finished to finally redraw the window.
        /// </summary>
        void EndUpdate();
    }
}
