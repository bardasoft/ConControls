/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using ConControls.ConsoleApi;
using ConControls.Helpers;
using ConControls.Logging;

namespace ConControls.Controls
{
    /// <summary>
    /// Base class for all console controls.
    /// </summary>
    [SuppressMessage("Design", "CA1001", Justification = "The DisposableBlock does not need to be disposed, its Dispose method has a different purpose.")]
    public abstract class ConsoleControl : IControlContainer
    {
        readonly DisposableBlock drawingInhibiter;
        string name;
        bool enabled = true;
        bool visible = true;

        ConsoleColor foregroundColor;
        ConsoleColor? focusedForegroundColor;
        ConsoleColor? disabledForegroundColor;
        ConsoleColor backgroundColor;
        ConsoleColor? focusedBackgroundColor;
        ConsoleColor? disabledBackgroundColor;
        BorderStyle borderStyle;
        BorderStyle? focusedBorderStyle;
        BorderStyle? disabledBorderStyle;
        ConsoleColor borderColor;
        ConsoleColor? focusedBorderColor;
        ConsoleColor? disabledBorderColor;

        Rectangle area;
        IControlContainer parent;
        int inhibitDrawing;

        /// <summary>
        /// Raised when the <see cref="Enabled"/> property has been changed.
        /// </summary>
        public event EventHandler? EnabledChanged;
        /// <summary>
        /// The <see cref="Visible"/> property of the control has been changed.
        /// </summary>
        public event EventHandler? VisibleChanged;
        /// <summary>
        /// Raised when the <see cref="Focused"/> property has been changed.
        /// </summary>
        public event EventHandler? FocusedChanged;
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
            get { lock(Window.SynchronizationLock) return name; }
            set
            {
                lock(Window.SynchronizationLock)
                    name = string.IsNullOrWhiteSpace(value) ? GetType().Name : value.Trim();
            }
        }
        /// <summary>
        /// Gets or sets wether the control is enabled or not.
        /// </summary>
        public bool Enabled
        {
            get { lock (Window.SynchronizationLock) return enabled && parent.Enabled; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (enabled == value) return;
                    enabled = value;
                    OnEnabledChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets wether the control should be visible (drawn) or not.
        /// </summary>
        public bool Visible
        {
            get { lock (Window.SynchronizationLock) return visible && parent.Visible; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (visible == value) return;
                    visible = value;
                    OnVisibleChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets wether this control is focused or not.
        /// Trying to set this to <code>true</code> though <see cref="CanFocus"/> returns <code>false</code>
        /// throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Focused"/> cannot be set to <code>true</code> when <see cref="CanFocus"/> returns <code>false</code>.</exception>
        public bool Focused
        {
            get
            {
                lock (Window.SynchronizationLock) return Window.FocusedControl == this;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == Focused) return;
                    if (value && !CanFocus())
                        throw Exceptions.CannotFocusUnFocusableControl(GetType().Name);
                    Window.FocusedControl = value ? this : null;
                    OnFocusedChanged();
                }
            }
        }
        /// <summary>
        /// Determines wether this control can be focused or not.
        /// </summary>
        /// <returns><code>true</code> if this control can take focues, <code>false</code> if not.</returns>
        public virtual bool CanFocus() => false;

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
        /// <inheritdoc />
        public Point Location
        {
            get
            {
                lock (Window.SynchronizationLock)
                    return area.Location;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (area.Location == value) return;
                    area = new Rectangle(value, area.Size);
                    OnAreaChanged();
                }
            }
        }
        /// <inheritdoc />
        public Size Size
        {
            get
            {
                lock (Window.SynchronizationLock)
                    return area.Size;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (area.Size == value) return;
                    area = new Rectangle(area.Location, value);
                    OnAreaChanged();
                }
            }
        }
        /// <summary>
        /// The <see cref="IConsoleWindow"/> that contains this control.
        /// </summary>
        public IConsoleWindow Window => parent.Window;
        /// <summary>
        /// The parent <see cref="ConsoleControl"/> that contains this control.
        /// The parent must be contained by the same <see cref="IConsoleWindow"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The parent is not part of the same <see cref="IConsoleWindow"/> or this control is the root element of the window..</exception>
        /// <exception cref="ArgumentNullException">The parent is <code>null</code>.</exception>
        public IControlContainer Parent
        {
            get { lock(Window.SynchronizationLock) return parent; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (parent == value) return;
                    if (value == null) throw Exceptions.ControlsMustBeContained();
                    if (value.Window != Window) throw Exceptions.DifferentWindow();
                    using (parent.DeferDrawing())
                    using (value.DeferDrawing())
                    { 
                        var oldparent = parent;
                        parent = value;
                        oldparent?.Controls.Remove(this);
                        parent?.Controls.Add(this);
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
        /// Determines if the control can currently be redrawn, depending on calls to <see cref="DeferDrawing"/>
        /// to this control or its parents.
        /// </summary>
        public bool DrawingInhibited => !visible || inhibitDrawing > 0 || parent.DrawingInhibited;

        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for foreground drawings.
        /// </summary>
        public ConsoleColor ForegroundColor
        {
            get { lock (Window.SynchronizationLock) return foregroundColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (foregroundColor == value) return;
                    foregroundColor = value;
                    OnForegroundColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for foreground drawings when the control is focused.
        /// If this is <code>null</code>, the <see cref="ForegroundColor"/> value will be used.
        /// </summary>
        public ConsoleColor? FocusedForegroundColor
        {
            get { lock (Window.SynchronizationLock) return focusedForegroundColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (focusedForegroundColor == value) return;
                    focusedForegroundColor = value;
                    OnForegroundColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for foreground drawings when the control is disabled.
        /// If this is <code>null</code>, the <see cref="ForegroundColor"/> value will be used.
        /// </summary>
        public ConsoleColor? DisabledForegroundColor
        {
            get { lock (Window.SynchronizationLock) return disabledForegroundColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (disabledForegroundColor == value) return;
                    disabledForegroundColor = value;
                    OnForegroundColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the background of this control.
        /// </summary>
        public ConsoleColor BackgroundColor
        {
            get { lock (Window.SynchronizationLock) return backgroundColor; }
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
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the background of this control
        /// when it is focused.
        /// If this is <code>null</code>, the <see cref="BackgroundColor"/> value will be used.
        /// </summary>
        public ConsoleColor? FocusedBackgroundColor
        {
            get { lock (Window.SynchronizationLock) return focusedBackgroundColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (focusedBackgroundColor == value) return;
                    focusedBackgroundColor = value;
                    OnBackgroundColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the background of this control
        /// when it is disabled.
        /// If this is <code>null</code>, the <see cref="BackgroundColor"/> value will be used.
        /// </summary>
        public ConsoleColor? DisabledBackgroundColor
        {
            get { lock (Window.SynchronizationLock) return disabledBackgroundColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (disabledBackgroundColor == value) return;
                    disabledBackgroundColor = value;
                    OnBackgroundColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the border of this control.
        /// </summary>
        public ConsoleColor BorderColor
        {
            get { lock (Window.SynchronizationLock) return borderColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (borderColor == value) return;
                    borderColor = value;
                    OnBorderColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the border of this control
        /// when it is focused.
        /// If this is <code>null</code>, the <see cref="BorderColor"/> value will be used.
        /// </summary>
        public ConsoleColor? FocusedBorderColor
        {
            get { lock (Window.SynchronizationLock) return focusedBorderColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (focusedBorderColor == value) return;
                    focusedBorderColor = value;
                    OnBorderColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for the border of this control
        /// when it is disabled.
        /// If this is <code>null</code>, the <see cref="BorderColor"/> value will be used.
        /// </summary>
        public ConsoleColor? DisabledBorderColor
        {
            get { lock (Window.SynchronizationLock) return disabledBorderColor; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (disabledBorderColor == value) return;
                    disabledBorderColor = value;
                    OnBorderColorChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="BorderStyle"/> of this control.
        /// </summary>
        public BorderStyle BorderStyle
        {
            get { lock (Window.SynchronizationLock) return borderStyle; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (borderStyle == value) return;
                    borderStyle = value;
                    OnBorderStyleChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="BorderStyle"/> of this control
        /// when it is focused.
        /// If this is <code>null</code>, the <see cref="ConsoleControl.BorderStyle"/> value will be used.
        /// </summary>
        public BorderStyle? FocusedBorderStyle
        {
            get { lock (Window.SynchronizationLock) return focusedBorderStyle; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (focusedBorderStyle == value) return;
                    focusedBorderStyle = value;
                    OnBorderStyleChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="BorderStyle"/> of this control
        /// when it is disabled.
        /// If this is <code>null</code>, the <see cref="ConsoleControl.BorderStyle"/> value will be used.
        /// </summary>
        public BorderStyle? DisabledBorderStyle
        {
            get { lock (Window.SynchronizationLock) return disabledBorderStyle; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (disabledBorderStyle == value) return;
                    disabledBorderStyle = value;
                    OnBorderStyleChanged();
                }
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="ConsoleControl"/>.
        /// </summary>
        /// <param name="parent">The parent <see cref="ConsoleControl"/> this control should be placed on.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent"/> is <code>null</code>.</exception>
        private protected ConsoleControl(IControlContainer parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            drawingInhibiter = new DisposableBlock(EndDeferDrawing);
            name = GetType().Name;

            foregroundColor = Window.ForegroundColor;
            backgroundColor = Window.BackgroundColor;
            borderColor = Window.BorderColor;
            borderStyle = Window.BorderStyle;

            Controls = new ControlCollection(Window);
            Controls.ControlAdded += OnControlAdded;
            Controls.ControlRemoved += OnControlRemoved;
            this.parent.Controls.Add(this);
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
        /// Invalidates this control to trigger redrawing.
        /// If <paramref name="onlyClientArea"/> is <code>true</code>, only
        /// the client area (without borders) will be invalidated.
        /// </summary>
        /// <param name="onlyClientArea">Set this to <code>true</code> if only the client area should be redrawn.
        /// This avoids drawing the border and background.</param>
        public void Invalidate(bool onlyClientArea = false)
        {
            if (onlyClientArea)
                DrawClientArea(Window.GetGraphics());
            else
                Draw();
        }
        /// <summary>
        /// Redraws the control.
        /// </summary>
        protected virtual void Draw()
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

                var color = EffectiveBackgroundColor;
                Logger.Log(DebugContext.Control | DebugContext.Drawing, $"drawing background ({color}.");
                graphics.DrawBackground(color, area);
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
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                var effectiveBorderStyle = EffectiveBorderStyle;
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

        /// <inheritdoc />
        public IDisposable DeferDrawing()
        {
            Interlocked.Increment(ref inhibitDrawing);
            return drawingInhibiter;
        }
        void EndDeferDrawing()
        {
            if (Interlocked.Decrement(ref inhibitDrawing) <= 0)
                Invalidate();
        }

        /// <summary>
        /// Gets the current foreground color based on the state of <see cref="Enabled"/> and <see cref="Focused"/> properties.
        /// </summary>
        protected virtual ConsoleColor EffectiveForegroundColor => Enabled
                                                                       ? Focused
                                                                             ? focusedForegroundColor ?? foregroundColor
                                                                             : foregroundColor
                                                                       : disabledForegroundColor ?? foregroundColor;
        /// <summary>
        /// Gets the current background color based on the state of <see cref="Enabled"/> and <see cref="Focused"/> properties.
        /// </summary>
        protected virtual ConsoleColor EffectiveBackgroundColor => Enabled
                                                                       ? Focused
                                                                             ? focusedBackgroundColor ?? backgroundColor
                                                                             : backgroundColor
                                                                       : disabledBackgroundColor ?? backgroundColor;
        /// <summary>
        /// Gets the current border color based on the state of <see cref="Enabled"/> and <see cref="Focused"/> properties.
        /// </summary>
        protected virtual ConsoleColor EffectiveBorderColor => Enabled
                                                                       ? Focused
                                                                             ? focusedBorderColor ?? borderColor
                                                                             : borderColor
                                                                       : disabledBorderColor ?? borderColor;
        /// <summary>
        /// Gets the current border style based on the state of <see cref="Enabled"/> and <see cref="Focused"/> properties.
        /// </summary>
        protected virtual BorderStyle EffectiveBorderStyle => Enabled
                                                                       ? Focused
                                                                             ? focusedBorderStyle ?? borderStyle
                                                                             : borderStyle
                                                                       : disabledBorderStyle ?? borderStyle;
        /// <summary>
        /// Determines the area of the control that can be used as "client" area.
        /// This base method e.g. remove the border from the total control area.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> representing the client area of this control.</returns>
        protected virtual Rectangle GetClientArea()
        {
            return EffectiveBorderStyle == BorderStyle.None
                       ? Area
                       : new Rectangle(Area.X + 1, Area.Y + 1, Area.Width - 2, Area.Height - 2);
        }

        /// <summary>
        /// Called when the <see cref="Parent"/> has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
            using (DeferDrawing())            
                ParentChanged?.Invoke(this, EventArgs.Empty);
        }
        void OnControlAdded(object sender, ControlCollectionChangedEventArgs e)
        {
            using (DeferDrawing())
            {
                        foreach (var addedControl in e.AddedControls)
                    addedControl.Parent = this;
                
                // TODO: add event handlers when necessary

                ControlAdded?.Invoke(this, e);
            }
        }
        void OnControlRemoved(object sender, ControlCollectionChangedEventArgs e)
        {
            using(DeferDrawing())
            {
                // TODO: remove eventhandlers when necessary
                ControlRemoved?.Invoke(this, e);
            }
        }
        /// <summary>
        /// Called when the <see cref="Focused"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnFocusedChanged()
        {
            using (DeferDrawing())
            {
                FocusedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="Enabled"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnEnabledChanged()
        {
            using(DeferDrawing())
            {
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="Visible"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            using (DeferDrawing())
            {
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="ForegroundColor"/>, <see cref="FocusedForegroundColor"/> or
        /// <see cref="DisabledForegroundColor"/> of this control have been changed.
        /// </summary>
        protected virtual void OnForegroundColorChanged()
        {
            Invalidate();
        }
        /// <summary>
        /// Called when the <see cref="BackgroundColor"/>, <see cref="FocusedBackgroundColor"/> or
        /// <see cref="DisabledBackgroundColor"/> of this control have been changed.
        /// </summary>
        protected virtual void OnBackgroundColorChanged()
        {
            Invalidate();
        }
        /// <summary>
        /// Called when the <see cref="BorderColor"/>, <see cref="FocusedBorderColor"/> or
        /// <see cref="DisabledBorderColor"/> of this control have been changed.
        /// </summary>
        protected virtual void OnBorderColorChanged()
        {
            Invalidate();
        }
        /// <summary>
        /// Called when the <see cref="BorderStyle"/>, <see cref="FocusedBorderStyle"/> or
        /// <see cref="DisabledBorderStyle"/> of this control have been changed.
        /// </summary>
        protected virtual void OnBorderStyleChanged()
        {
            Invalidate();
        }
        /// <summary>
        /// Called when the <see cref="Area"/> of this control has been changed.
        /// </summary>
        protected virtual void OnAreaChanged()
        {
            using (DeferDrawing())
            {
                AreaChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
