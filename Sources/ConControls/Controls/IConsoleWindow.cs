/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using System.Text;
using ConControls.Controls.Drawing;

namespace ConControls.Controls
{
    /// <summary>
    /// Interface to a <see cref="ConsoleWindow"/>.
    /// </summary>
    public interface IConsoleWindow : IControlContainer, IDisposable
    {
        /// <summary>
        /// Raised when a console input key event has been received.
        /// </summary>
        event EventHandler<KeyEventArgs>? KeyEvent;
        /// <summary>
        /// Raised when a console input mouse event has been received.
        /// </summary>
        event EventHandler<MouseEventArgs>? MouseEvent;
        /// <summary>
        /// Raised when an output to stdout has been received.
        /// </summary>
        event EventHandler<StdOutEventArgs>? StdOutEvent;
        /// <summary>
        /// Raised when an output to stdout has been received.
        /// </summary>
        event EventHandler<StdErrEventArgs>? StdErrEvent;
        
        /// <summary>
        /// Raised when this console window is disposed of.
        /// </summary>
        event EventHandler? Disposed;

        /// <summary>
        /// Gets the original console output encoding
        /// </summary>
        Encoding OutputEncoding { get; }

        /// <summary>
        /// The title of the console window.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Gets the maximum size of the console window based on the current font and the size of the display.
        /// </summary>
        Size MaximumSize { get; }
        /// <summary>
        /// Gets the default cursor size to use (0-100).
        /// </summary>
        int CursorSize { get; set; }
        /// <summary>
        /// Gets or sets the default foreground color.
        /// </summary>
        ConsoleColor ForegroundColor { get; set; }
        /// <summary>
        /// Gets or sets the default background color.
        /// </summary>
        ConsoleColor BackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the default border color.
        /// </summary>
        ConsoleColor BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the default border style.
        /// </summary>
        BorderStyle BorderStyle { get; set; }
        /// <summary>
        /// Gets or sets the currently focused control on this window.
        /// </summary>
        ConsoleControl? FocusedControl { get; set; }

        /// <summary>
        /// The <see cref="FrameCharSets"/> provider to use to draw frames.
        /// </summary>
        FrameCharSets FrameCharSets { get; set; }

        /// <summary>
        /// The window has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// A synchronization object to synchronize conosle operations.
        /// </summary>
        object SynchronizationLock { get; }

        /// <summary>
        /// Provides a <see cref="IConsoleGraphics"/> to draw on this console window.
        /// </summary>
        /// <returns>An <see cref="IConsoleGraphics"/> iterface to draw on this window.</returns>
        IConsoleGraphics GetGraphics();
    }
}
