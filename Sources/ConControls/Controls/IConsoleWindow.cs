/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
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
        /// Raised when this console window is disposed of.
        /// </summary>
        event EventHandler? Disposed;

        /// <summary>
        /// The title of the console window.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Gets or sets the default cursor size to use (0-100) for
        /// newly created focusable controls.
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
        /// Sets the active screen buffer.
        /// If <paramref name="show"/> is <c>true</c>, this window is set
        /// as active screen buffer. If <paramref name="show"/> is <c>false</c>, the
        /// original console screen buffer will be activated.
        /// </summary>
        /// <param name="show"><c>true</c> to show this window, <c>false</c> to display the original console screen buffer of the process.</param>
        void SetActiveScreen(bool show);

        /// <summary>
        /// Provides a <see cref="IConsoleGraphics"/> to draw on this console window.
        /// </summary>
        /// <returns>An <see cref="IConsoleGraphics"/> iterface to draw on this window.</returns>
        IConsoleGraphics GetGraphics();

        /// <summary>
        /// Selects the next focusable control.
        /// </summary>
        /// <returns>The focused control, or <c>null</c> if no control could be focused.</returns>
        ConsoleControl? FocusNext();
        /// <summary>
        /// Selects the previous focusable control.
        /// </summary>
        /// <returns>The focused control, or <c>null</c> if no control could be focused.</returns>
        ConsoleControl? FocusPrevious();
    }
}
