using System;
using System.Drawing;

namespace ConControls.Controls
{
    /// <summary>
    /// An interface for classes that can contain console controls.
    /// </summary>
    public interface IControlContainer : IDisposable
    {
        /// <summary>
        /// Raised when the <see cref="Area"/> (or <see cref="Location"/> or <see cref="Size"/> have changed.
        /// </summary>
        event EventHandler? AreaChanged;

        /// <summary>
        /// The main <see cref="IConsoleWindow"/> this <see cref="IControlContainer"/> belongs to.
        /// </summary>
        IConsoleWindow Window { get; }
        /// <summary>
        /// The parent <see cref="ConsoleControl"/> that contains this control.
        /// The parent must be contained by the same <see cref="IConsoleWindow"/>.
        /// If this propery is <c>null</c>, the control is not displayed.
        /// </summary>
        /// <exception cref="InvalidOperationException">The parent is not part of the same <see cref="IConsoleWindow"/> or this control is the root element of the window..</exception>
        IControlContainer? Parent { get; }
        /// <summary>
        /// The location of this <see cref="IControlContainer"/> in character columns and rows
        /// from the upper left corner of the console screen buffer.
        /// </summary>
        Point Location { get; }
        /// <summary>
        /// The size of this <see cref="IControlContainer"/> in character columns and rows.
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// A <see cref="Rectangle"/> representing the area (in character columns and rows) this <see cref="IControlContainer"/>
        /// occupies in the console screen buffer.
        /// </summary>
        Rectangle Area { get; }

        /// <summary>
        /// The collection of child controls in this <see cref="IControlContainer"/>.
        /// </summary>
        ControlCollection Controls { get; }
        
        /// <summary>
        /// Determines if drawing is currently allowed or inhibited via <see cref="DeferDrawing"/>
        /// or the <see cref="Visible"/> property.
        /// </summary>
        bool DrawingInhibited { get; }
        /// <summary>
        /// Gets wether this control or <see cref="IControlContainer"/> is enabled.
        /// </summary>
        bool Enabled { get; }
        /// <summary>
        /// Gets wether this control or <see cref="IControlContainer"/> is visible.
        /// </summary>
        bool Visible { get; }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for foreground drawings.
        /// </summary>
        ConsoleColor? ForegroundColor { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the background of this control.
        /// </summary>
        /// <remarks>
        /// <para>If this property is <c>null</c>, the parent's setting (or finally the <see cef="Window"/>'s default setting) will be used.</para>
        /// </remarks>
        ConsoleColor? BackgroundColor { get; set; }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the border of this control.
        /// </summary>
        /// <remarks>
        /// <para>If this property is <c>null</c>, the parent's setting (or finally the <see cef="Window"/>'s default setting) will be used.</para>
        /// </remarks>
        ConsoleColor? BorderColor { get; set; }
        /// <summary>
        /// Gets or sets the cursor size for this control.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value describes the size of the text cursor. It should be between <c>zero</c> and <c>100</c>.
        /// </para>
        /// <para>
        /// This property is only used by the <see cref="Window"/> if this control is
        /// currently focused.
        /// </para>
        /// <para>If this property is <c>null</c>, the parent's setting (or finally the <see cef="Window"/>'s default setting) will be used.</para>
        /// </remarks>
        int? CursorSize { get; set; }

        /// <summary>
        /// Defers any drawing operation until the return value has been disposed of again.
        /// </summary>
        /// <remarks>Use this to surround a block of code that changes multiple properties of this
        /// <see cref="IControlContainer"/> or its children. This avoids redrawing the control for
        /// each changed property, and instead redraws the whole container when the returned value
        /// is disposed.
        /// <code language="c#">
        /// ConsoleControl control = ...;
        /// using(control.DeferDrawing())
        /// {
        ///     control.ForegroundColor = ConsoleColor.Green;
        ///     control.BackgroundColor = ConsoleColor.Black;
        /// }
        /// </code>
        /// At the end of this block, drawing will be resumed again.
        /// </remarks>
        /// <returns>An <see cref="IDisposable"/> that on calling <see cref="IDisposable.Dispose"/> releases
        /// the drawing barriere of this window.</returns>
        IDisposable DeferDrawing();

        /// <summary>
        /// Converts a <see cref="Point"/> from console coordinates to client coordinates.
        /// </summary>
        /// <param name="consolePoint">The <see cref="Point"/> in console coordinates to convert to client coordinates.</param>
        /// <returns>The converted <see cref="Point"/> in client coordinates.</returns>
        Point PointToClient(Point consolePoint);
        /// <summary>
        /// Converts a <see cref="Point"/> from client coordinates to console coordinates.
        /// </summary>
        /// <param name="clientPoint">The <see cref="Point"/> in client coordinates to convert to console coordinates.</param>
        /// <returns>The converted <see cref="Point"/> in console coordinates.</returns>
        Point PointToConsole(Point clientPoint);
    }
}
