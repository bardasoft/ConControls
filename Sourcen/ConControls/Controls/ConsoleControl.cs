/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using System.Threading;
using ConControls.ConsoleApi;
using ConControls.Logging;

namespace ConControls.Controls
{
    /// <summary>
    /// Base class for all console controls.
    /// </summary>
    public class ConsoleControl
    {
        string name;
        bool visible = true;
        Rectangle area;
        ConsoleControl? parent;
        int inhibitDrawing;
        BorderStyle? borderStyle;
        ConsoleColor? borderColor;
        ConsoleColor? foreColor;
        ConsoleColor? backgroundColor;

        /// <summary>
        /// The <see cref="Visible"/> property of the control has been changed.
        /// </summary>
        public event EventHandler? VisibleChanged;
        /// <summary>
        /// The <see cref="Area"/> of the control has been changed.
        /// </summary>
        public event EventHandler? AreaChanged;
        /// <summary>
        /// The <see cref="Parent"/> of the control has been changed.
        /// </summary>
        public event EventHandler? ParentChanged;
        /// <summary>
        /// A <see cref="ConsoleControl"/> has been added to the <see cref="Controls"/> collection
        /// of this control.
        /// </summary>
        public event EventHandler<ControlCollectionChangedEventArgs>? ControlAdded;
        /// <summary>
        /// A <see cref="ConsoleControl"/> has been removed from the <see cref="Controls"/> collection
        /// of this control.
        /// </summary>
        public event EventHandler<ControlCollectionChangedEventArgs>? ControlRemoved;

        /// <summary>
        /// The name of this control (merely for debug identification).
        /// </summary>
        public string Name
        {
            get => name;
            set => name = string.IsNullOrWhiteSpace(value) ? GetType().Name : value.Trim();
        }

        /// <summary>
        /// Gets or sets wether the control should be visible (drawn) or not.
        /// </summary>
        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                OnVisibleChanged();
            }
        }
        /// <summary>
        /// The effective total area of the control.
        /// This is the area the control effectivly fills in the console screen buffer
        /// after applying layout and including borders.
        /// </summary>
        public Rectangle Area
        {
            get
            {
                lock (Window.SynchronizationLock) return area;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == area) return;
                    area = value;
                    OnAreaChanged();
                }
            }
        }
        /// <summary>
        /// The <see cref="IConsoleWindow"/> that contains this control.
        /// </summary>
        public IConsoleWindow Window { get; }
        /// <summary>
        /// The parent <see cref="ConsoleControl"/> that contains this control.
        /// The parent must be contained by the same <see cref="IConsoleWindow"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The parent is not part of the same <see cref="IConsoleWindow"/> or this control is the root element of the window..</exception>
        /// <exception cref="ArgumentNullException">The parent is <code>null</code>.</exception>
        public ConsoleControl? Parent
        {
            get => parent;
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (parent == value) return;
                    if (value != null && value.Window != Window) throw Exceptions.DifferentWindow();

                    if (parent == null || value == null)
                        Window.BeginUpdate();
                    parent?.BeginUpdate();
                    value?.BeginUpdate();
                    try
                    {
                        if (parent == null)
                            Window.Controls.Remove(this);
                        else
                            parent.Controls.Remove(this);
                        if (value == null)
                            Window.Controls.Add(this);
                        else
                            value.Controls.Add(this);
                    }
                    finally
                    {
                        if (parent == null || value == null)
                            Window.EndUpdate();
                        parent?.EndUpdate();
                        value?.EndUpdate();
                        parent = value;
                    }

                    OnParentChanged();
                }
            }

        }
        /// <summary>
        /// The collection of <see cref="ConsoleControl"/>s contained by this control.
        /// </summary>
        public ControlCollection Controls { get; }

        /// <summary>
        /// Determines if the control can currently be redrawn, depending on calls to <see cref="BeginUpdate"/> and
        /// <see cref="EndUpdate"/> to this control or its parents.
        /// </summary>
        public bool DrawingInhibited => !visible || inhibitDrawing > 0 || parent?.DrawingInhibited == true || Window.DrawingInhibited;

        /// <summary>
        /// The <see cref="BorderStyle"/> of this control.
        /// If this is <code>null</code> the <see cref="Parent"/>'s border style will be used.
        /// If this is <code>null</code>, too, no border will be drawn.
        /// </summary>
        public BorderStyle? BorderStyle
        {
            get => borderStyle;
            set
            {
                if (value == borderStyle) return;
                borderStyle = value;
                OnBorderStyleChanged();
            }
        }
        /// <summary>
        /// The color of this control's border.
        /// If this is <code>null</code> the <see cref="Parent"/>'s border color will be used.
        /// If this is <code>null</code>, too, the default (<see cref="ConsoleColor.Yellow"/>) will be used.
        /// </summary>
        public ConsoleColor? BorderColor
        {
            get => borderColor;
            set
            {
                if (value == borderColor) return;
                borderColor = value;
                OnBorderColorChanged();
            }
        }

        /// <summary>
        /// The foreground color of this control.
        /// If this is <code>null</code> the <see cref="Parent"/>'s foreground color will be used.
        /// If this is <code>null</code>, too, the default (<see cref="ConsoleColor.Gray"/> will be used.
        /// </summary>
        public ConsoleColor? ForeColor
        {
            get => foreColor;
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (foreColor == value) return;
                    foreColor = value;
                    OnForeColorChanged();
                }
            }
        }
        /// <summary>
        /// The background color of this control.
        /// If this is <code>null</code> the <see cref="Parent"/>'s background color will be used.
        /// If this is <code>null</code>, too, the default (<see cref="ConsoleColor.Black"/> will be used.
        /// </summary>
        public ConsoleColor? BackgroundColor
        {
            get => backgroundColor;
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (backgroundColor == value) return;
                    backgroundColor = value;
                    OnBackgroundColorChanged();
                }
            }
        }

        /// <summary>
        /// Creates a new <see cref="ConsoleControl"/> and adds it to the
        /// <paramref name="window"/>'s control collection.
        /// </summary>
        /// <param name="window">The <see cref="IConsoleWindow"/> this control should be placed on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <code>null</code>.</exception>
        private protected ConsoleControl(IConsoleWindow window)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
            name = GetType().Name;
            Controls = new ControlCollection(Window);
            Controls.ControlAdded += OnControlAdded;
            Controls.ControlRemoved += OnControlRemoved;
            window.Controls.Add(this);
        }
        /// <summary>
        /// Initializes an instance of <see cref="ConsoleControl"/>.
        /// </summary>
        /// <param name="window">The <see cref="IConsoleWindow"/> this control should be placed on.</param>
        /// <param name="parent">The parent <see cref="ConsoleControl"/> this control should be placed on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> or <paramref name="parent"/> is <code>null</code>.</exception>
        private protected ConsoleControl(IConsoleWindow window, ConsoleControl parent)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            name = GetType().Name;
            Controls = new ControlCollection(Window);
            Controls.ControlAdded += OnControlAdded;
            Controls.ControlRemoved += OnControlRemoved;
            parent.Controls.Add(this);
        }

        /// <summary>
        /// Checks if the <see cref="IConsoleWindow"/> containing this control has already
        /// been disposed of and throws an <see cref="ObjectDisposedException"/> if it is.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        protected void CheckDisposed()
        {
            if (Window.IsDisposed) throw Exceptions.WindowDisposed();
        }

        /// <summary>
        /// Redraws the control.
        /// </summary>
        public void Draw()
        {
            Logger.Log(DebugContext.Control | DebugContext.Drawing, "parameterless called.");
            lock (Window.SynchronizationLock)
            {
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Getting graphics object.");
                var graphics = Window.GetGraphics();
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Start drawing.");
                Draw(graphics);
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Flushing.");
                graphics.Flush();
            }
        }
        /// <summary>
        /// Draws the control onto the console screen buffer.
        /// When overwriting this method, make sure to use the <see cref="IConsoleWindow.SynchronizationLock"/>
        /// to synchronize threads and to call <see cref="CheckDisposed"/> to check if the window has not yet
        /// been disposed of.
        /// The <paramref name="graphics"/> buffer should be flushed by the caller.
        /// </summary>
        /// <param name="graphics">An <see cref="IConsoleGraphics"/> that performs the drawing operations on
        /// the screen buffer.</param>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        public virtual void Draw(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            Logger.Log(DebugContext.Control | DebugContext.Drawing, "with graphics called.");

            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing background.");
                DrawBackground(graphics);
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing border.");
                DrawBorder(graphics);
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing client area.");
                DrawClientArea(graphics);
            }
        }
        /// <summary>
        /// Draws the background onto the console screen buffer.
        /// When overwriting this method, make sure to use the <see cref="IConsoleWindow.SynchronizationLock"/>
        /// to synchronize threads and to call <see cref="CheckDisposed"/> to check if the window has not yet
        /// been disposed of.
        /// The <paramref name="graphics"/> buffer should be flushed by the caller.
        /// </summary>
        /// <param name="graphics">An <see cref="IConsoleGraphics"/> that performs the drawing operations on
        /// the screen buffer.</param>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        protected virtual void DrawBackground(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                var effectiveBackgroundColor = EffectiveBackgroundColor;
                Logger.Log(DebugContext.Control | DebugContext.Drawing, $"drawing background ({effectiveBackgroundColor}.");
                graphics.DrawBackground(backgroundColor ?? parent?.backgroundColor ?? Window.BackgroundColor, area);
            }
        }
        /// <summary>
        /// Draws the border of the control onto the console screen buffer and returns the remaining client area.
        /// When overwriting this method, make sure to use the <see cref="IConsoleWindow.SynchronizationLock"/>
        /// to synchronize threads and to call <see cref="CheckDisposed"/> to check if the window has not yet
        /// been disposed of.
        /// The <paramref name="graphics"/> buffer should be flushed by the caller.
        /// </summary>
        /// <param name="graphics">An <see cref="IConsoleGraphics"/> that performs the drawing operations on
        /// the screen buffer.</param>
        /// <returns>A <see cref="Rectangle"/> representing the available client area.</returns>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        protected virtual void DrawBorder(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            lock (Window.SynchronizationLock)
            {
                var effectiveBorderStyle = EffectiveBorderStyle;
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                var effectiveBorderColor = EffectiveBorderColor;
                var effectiveBackgroundColor = EffectiveBackgroundColor;

                Logger.Log(DebugContext.Control | DebugContext.Drawing, $"drawing border ({effectiveBorderColor} on {effectiveBackgroundColor}, {effectiveBorderStyle}).");
                graphics.DrawBorder(effectiveBackgroundColor, effectiveBorderColor, effectiveBorderStyle, area);
            }
        }
        /// <summary>
        /// Draws the client area and child controls onto the console screen buffer.
        /// When overwriting this method, make sure to use the <see cref="IConsoleWindow.SynchronizationLock"/>
        /// to synchronize threads and to call <see cref="CheckDisposed"/> to check if the window has not yet
        /// been disposed of.
        /// The <paramref name="graphics"/> buffer should be flushed by the caller.
        /// </summary>
        /// <param name="graphics">An <see cref="IConsoleGraphics"/> that performs the drawing operations on
        /// the screen buffer.</param>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        protected virtual void DrawClientArea(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing children");
                foreach (var child in Controls)
                    child.Draw(graphics);
            }
        }

        /// <summary>
        /// Inhibits any redrawing etc. until <see cref="EndUpdate"/> is called.
        /// Use this to avoid multiple redrawings while updating multiple properties.
        /// </summary>
        public void BeginUpdate()
        {
            Interlocked.Increment(ref inhibitDrawing);
        }
        /// <summary>
        /// Finishes an update sequence. Call <see cref="BeginUpdate"/> before you
        /// update multiple properties to avoid multiple redrawings, and call <see cref="EndUpdate"/>
        /// when you are finished and want to redraw the control.
        /// </summary>
        public void EndUpdate()
        {
            if (Interlocked.Decrement(ref inhibitDrawing) <= 0)
                Draw();
        }

        /// <summary>
        /// Determines the area of the control that can be used as "client" area.
        /// This base method e.g. remove the border from the total control area.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> representing the client area of this control.</returns>
        protected virtual Rectangle GetClientArea()
        {
            return EffectiveBorderStyle == ConControls.Controls.BorderStyle.None
                       ? Area
                       : new Rectangle(Area.X + 1, Area.Y + 1, Area.Width - 2, Area.Height - 2);
        }

        /// <summary>
        /// Called when the <see cref="Parent"/> has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
            BeginUpdate();
            try
            {
                ParentChanged?.Invoke(this, EventArgs.Empty);
            }
            finally { EndUpdate(); }
        }
        void OnControlAdded(object sender, ControlCollectionChangedEventArgs e)
        {
            BeginUpdate();
            try
            {
                foreach (var addedControl in e.AddedControls)
                    addedControl.Parent = this;
                // add event handlers when necessary

                ControlAdded?.Invoke(this, e);
            }
            finally { EndUpdate(); }

        }
        void OnControlRemoved(object sender, ControlCollectionChangedEventArgs e)
        {
            BeginUpdate();
            try
            {
                // remove eventhandlers when necessary

                ControlRemoved?.Invoke(this, e);
            }
            finally
            {
                EndUpdate();
            }
        }
        /// <summary>
        /// Called when the <see cref="ForeColor"/> of this control has been changed.
        /// </summary>
        protected virtual void OnForeColorChanged()
        {
            Draw();
        }
        /// <summary>
        /// Called when the <see cref="BackgroundColor"/> of this control has been changed.
        /// </summary>
        protected virtual void OnBackgroundColorChanged()
        {
            Draw();
        }
        void OnVisibleChanged()
        {
            lock (Window.SynchronizationLock)
            {
                if (parent == null) Window.Draw();
                else parent.Draw();
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="Area"/> of this control has been changed.
        /// </summary>
        protected virtual void OnAreaChanged()
        {
            BeginUpdate();
            try
            {
                AreaChanged?.Invoke(this, EventArgs.Empty);
                if (Parent == null)
                    Window.Draw();
                else
                    Parent.Draw();
            }
            finally
            {
                EndUpdate();
            }
        }
        /// <summary>
        /// Called when the <see cref="BorderStyle"/> of this control has been changed.
        /// </summary>
        protected virtual void OnBorderStyleChanged()
        {
            Draw();
        }
        /// <summary>
        /// Called when the <see cref="BorderColor"/> of this control has been changed.
        /// </summary>
        protected virtual void OnBorderColorChanged()
        {
            Draw();
        }

        /// <summary>
        /// Gets the effective foreground color (applying transparency).
        /// </summary>
        protected ConsoleColor EffectiveForeColor => foreColor ?? Parent?.EffectiveForeColor ?? Window.ForeColor;
        /// <summary>
        /// Gets the effective background color (applying transparency).
        /// </summary>
        protected ConsoleColor EffectiveBackgroundColor => backgroundColor ?? Parent?.EffectiveBackgroundColor ?? Window.BackgroundColor;
        /// <summary>
        /// Gets the effective border color (applying transparency).
        /// </summary>
        protected ConsoleColor EffectiveBorderColor => borderColor ?? Parent?.EffectiveBorderColor ?? ConsoleColor.Yellow;
        /// <summary>
        /// Gets the effective border style (applying transparency).
        /// </summary>
        protected BorderStyle EffectiveBorderStyle => borderStyle ?? Parent?.EffectiveBorderStyle ?? ConControls.Controls.BorderStyle.None;
    }
}
