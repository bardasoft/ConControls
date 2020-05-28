/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Threading.Tasks;
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
        int DefaultCursorSize { get; set; }
        /// <summary>
        /// Gets or sets the default foreground color.
        /// </summary>
        ConsoleColor DefaultForegroundColor { get; set; }
        /// <summary>
        /// Gets or sets the default background color.
        /// </summary>
        ConsoleColor DefaultBackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the default border color.
        /// </summary>
        ConsoleColor DefaultBorderColor { get; set; }
        /// <summary>
        /// Gets or sets the default border style.
        /// </summary>
        BorderStyle DefaultBorderStyle { get; set; }
        /// <summary>
        /// Gets or sets the currently focused control on this window.
        /// </summary>
        ConsoleControl? FocusedControl { get; set; }

        /// <summary>
        /// The <see cref="FrameCharSets"/> provider to use to draw frames.
        /// </summary>
        FrameCharSets FrameCharSets { get; set; }

        /// <summary>
        /// The key combination to switch between the window and the original console screen buffer.
        /// </summary>
        KeyCombination? SwitchConsoleBuffersKey { get; set; }
        /// <summary>
        /// The key combination to close the window.
        /// </summary>
        KeyCombination? CloseWindowKey { get; set; }
        /// <summary>
        /// Gets or sets wether this window is the active console screen buffer.
        /// If set to <c>true</c> this window is the active screen. If set to <c>false</c> the
        ///  original console screen buffer will be activated.
        /// </summary>
        bool ActiveScreen { get; set; }

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

        /// <summary>
        /// The exit code the window was closed with.
        /// </summary>
        int ExitCode { get; }

        /// <summary>
        /// Closes (and diposes) the window and sets the given exit code.
        /// </summary>
        /// <param name="exitCode">The exit code for the window.</param>
        void Close(int exitCode = 0);

        /// <summary>
        /// Waits asynchronously for the window to be closed/disposed.
        /// </summary>
        /// <returns>A task that is completed when the window is closed and returns the exit code.</returns>
        Task<int> WaitForCloseAsync();
    }
}
