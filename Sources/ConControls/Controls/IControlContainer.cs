using System;
using System.Drawing;

namespace ConControls.Controls
{
    /// <summary>
    /// An interface for classes that can contain console controls.
    /// </summary>
    public interface IControlContainer
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
        /// The location of this <see cref="IControlContainer"/> in character columns and rows
        /// from the upper left corner of the console screen buffer.
        /// </summary>
        Point Location { get; }
        /// <summary>
        /// The size of this <see cref="IControlContainer"/> in character columns and rows.
        /// </summary>
        Size Size { get; }
        /// <summary>
        /// A <see cref="Rectangle"/> representing the area (in charactre columns and rows) this <see cref="IControlContainer"/>
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
        /// Defers any drawing operation until the return value has been disposed of again.
        /// </summary>
        /// <remarks>Use this to surround a block of code that changes multiple properties of this
        /// <see cref="IControlContainer"/> or its children.
        /// <p>
        /// using(DeferDrawing){....}
        /// </p>
        /// At the end of this block, drawing will be resumed again.
        /// </remarks>
        /// <returns>An <see cref="IDisposable"/> that on calling <see cref="IDisposable.Dispose"/> releases
        /// the drawing barriere of this window.</returns>
        IDisposable DeferDrawing();
    }
}
