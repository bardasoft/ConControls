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
using ConControls.Controls.Drawing;
using ConControls.Helpers;
using ConControls.Logging;

namespace ConControls.Controls
{
    /// <summary>
    /// Base class for all console controls.
    /// </summary>
    /// <threadsafety>
    /// All public properties and methods of this class are synchronized using the
    /// <see cref="IConsoleWindow.SynchronizationLock"/> of the control's <see cref="Window"/>.<br/>
    /// When inheriting custom controls, this pattern of synchronizing to the window's synchronization object
    /// should be applied.
    /// </threadsafety>
    [SuppressMessage("Design", "CA1001", Justification = "The DisposableBlock does not need to be disposed, its Dispose method has a different purpose.")]
    public abstract class ConsoleControl : IControlContainer
    {
#pragma warning disable CA2213
        readonly DisposableBlock drawingInhibiter;
#pragma warning restore CA2213

        int disposed;
        string name;
        bool enabled = true;
        bool visible = true;
        Point cursorPosition;
        int cursorSize;
        bool cursorVisible;

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
        /// The <see cref="Name"/> of the control has been changed.
        /// </summary>
        public event EventHandler? NameChanged;
        /// <summary>
        /// The <see cref="CursorPosition"/> of the control has been changed.
        /// </summary>
        public event EventHandler? CursorPositionChanged;
        /// <summary>
        /// The <see cref="CursorSize"/> of the control has been changed.
        /// </summary>
        public event EventHandler? CursorSizeChanged;
        /// <summary>
        /// The <see cref="CursorVisible"/> of the control has been changed.
        /// </summary>
        public event EventHandler? CursorVisibleChanged;

        /// <summary>
        /// The name of this control (merely for debug identification).
        /// </summary>
        public string Name
        {
            get { lock(Window.SynchronizationLock) return name; }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (name == value) return;
                    name = string.IsNullOrWhiteSpace(value) ? GetType().Name : value.Trim();
                    OnNameChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets wether the control is enabled or not.
        /// </summary>
        public virtual bool Enabled
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
        public virtual bool Visible
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
        /// Trying to set this to <c>true</c> though <see cref="CanFocus"/> returns <c>false</c>
        /// throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="Focused"/> cannot be set to <c>true</c> when <see cref="CanFocus"/> returns <c>false</c>.</exception>
        public virtual bool Focused
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
                    if (value && !CanFocus)
                        throw Exceptions.CannotFocusUnFocusableControl(GetType().Name);
                    Window.FocusedControl = value ? this : null;
                    OnFocusedChanged();
                }
            }
        }
        /// <summary>
        /// Determines wether this control can be focused or not.
        /// </summary>
        /// <returns><c>true</c> if this control can take focues, <c>false</c> if not.</returns>
        public virtual bool CanFocus => false;

        /// <summary>
        /// The effective total area of the control.
        /// This is the area the control effectivly fills in the console screen buffer
        /// after applying layout and including borders.
        /// </summary>
        public virtual Rectangle Area
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
        public virtual Point Location
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
        public virtual Size Size
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
        /// <exception cref="ArgumentNullException">The parent is <c>null</c>.</exception>
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
                        oldparent.Controls.Remove(this);
                        parent.Controls.Add(this);
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
        /// Gets or sets the cursor position for this control in character coordinates of the console
        /// screen buffer.
        /// </summary>
        /// <remarks>
        /// This property is only used by the <see cref="IConsoleWindow"/> if this control is currently
        /// focused.
        /// </remarks>
        public virtual Point CursorPosition
        {
            get
            {
                lock (Window.SynchronizationLock) return cursorPosition;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == cursorPosition) return;
                    cursorPosition = value;
                    OnCursorPositionChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the cursor size for this control.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This value describes the size of the text cursor. It should be between <c>zero</c> and <c>100</c>.
        /// </para>
        /// <para>
        /// This property is only used by the <see cref="IConsoleWindow"/> if this control is
        /// currently focused.
        /// </para>
        /// </remarks>
        public virtual int CursorSize
        {
            get
            {
                lock (Window.SynchronizationLock) return cursorSize;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == cursorSize) return;
                    cursorSize = value;
                    OnCursorSizeChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets wether the cursor is visible on this control.
        /// screen buffer.
        /// </summary>
        /// <remarks>
        /// This property is only used by the <see cref="IConsoleWindow"/> if this control is
        /// currently focused.
        /// </remarks>
        public virtual bool CursorVisible
        {
            get
            {
                lock (Window.SynchronizationLock) return cursorVisible;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == cursorVisible) return;
                    cursorVisible = value;
                    OnCursorVisibleChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the <see cref="ConsoleColor"/> to use for foreground drawings.
        /// </summary>
        public virtual ConsoleColor ForegroundColor
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
        /// If this is <c>null</c>, the <see cref="ForegroundColor"/> value will be used.
        /// </summary>
        public virtual ConsoleColor? FocusedForegroundColor
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
        /// If this is <c>null</c>, the <see cref="ForegroundColor"/> value will be used.
        /// </summary>
        public virtual ConsoleColor? DisabledForegroundColor
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
        public virtual ConsoleColor BackgroundColor
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
        /// If this is <c>null</c>, the <see cref="BackgroundColor"/> value will be used.
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
        /// If this is <c>null</c>, the <see cref="BackgroundColor"/> value will be used.
        /// </summary>
        public virtual ConsoleColor? DisabledBackgroundColor
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
        public virtual ConsoleColor BorderColor
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
        /// If this is <c>null</c>, the <see cref="BorderColor"/> value will be used.
        /// </summary>
        public virtual ConsoleColor? FocusedBorderColor
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
        /// If this is <c>null</c>, the <see cref="BorderColor"/> value will be used.
        /// </summary>
        public virtual ConsoleColor? DisabledBorderColor
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
        public virtual BorderStyle BorderStyle
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
        /// Gets or sets the <see cref="ConControls.Controls.BorderStyle"/> of this control
        /// when it is focused.
        /// If this is <c>null</c>, the <see cref="BorderStyle"/> value will be used.
        /// </summary>
        public virtual BorderStyle? FocusedBorderStyle
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
        /// Gets or sets the <see cref="ConControls.Controls.BorderStyle"/> of this control
        /// when it is disabled.
        /// If this is <c>null</c>, the <see cref="BorderStyle"/> value will be used.
        /// </summary>
        public virtual BorderStyle? DisabledBorderStyle
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
        /// <exception cref="ArgumentNullException"><paramref name="parent"/> is <c>null</c>.</exception>
        private protected ConsoleControl(IControlContainer parent)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            drawingInhibiter = new DisposableBlock(EndDeferDrawing);
            name = GetType().Name;
            cursorSize = Window.CursorSize;

            Window.KeyEvent += OnWindowKeyEvent;
            Window.MouseEvent += OnWindowMouseEvent;
            Window.StdOutEvent += OnWindowStdOutEvent;
            Window.StdErrEvent += OnWindowStdErrEvent;

            foregroundColor = Window.ForegroundColor;
            backgroundColor = Window.BackgroundColor;
            borderColor = Window.BorderColor;
            borderStyle = Window.BorderStyle;

            Controls = new ControlCollection(Window);
            Controls.ControlCollectionChanged += OnControlCollectionChanged;
            this.parent.Controls.Add(this);
        }


        /// <summary>
        /// Disposes of any used resources and disconnects from the <see cref="Window"/>.
        /// </summary>
        [SuppressMessage("Design", "CA1063", Justification = "Analyzer mistake")]
        public void Dispose()
        {
            lock(Window.SynchronizationLock)
                Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Disposes of any used resources and disconnects from the <see cref="Window"/>.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref disposed, 1, 0) != 0) return;
            if (!disposing) return;
            Window.KeyEvent -= OnWindowKeyEvent;
            Window.MouseEvent -= OnWindowMouseEvent;
            Window.StdOutEvent -= OnWindowStdOutEvent;
            Window.StdErrEvent -= OnWindowStdErrEvent;
        }

        /// <summary>
        /// Checks if the <see cref="IConsoleWindow"/> containing this control has already
        /// been disposed of and throws an <see cref="ObjectDisposedException"/> if it is.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        protected void CheckDisposed()
        {
            if (Window.IsDisposed) throw Exceptions.WindowDisposed();
            if (disposed > 0) throw Exceptions.ControlDisposed(name);
        }

        /// <inheritdoc />
        public Point PointToConsole(Point clientPoint) => Parent.PointToConsole(Point.Add(clientPoint, (Size)Location));
        /// <inheritdoc />
        public Point PointToClient(Point consolePoint) => Point.Subtract(Parent.PointToClient(consolePoint), (Size)Location);

        /// <summary>
        /// Invalidates this control to trigger redrawing.
        /// If <paramref name="onlyClientArea"/> is <c>true</c>, only
        /// the client area (without borders) will be invalidated.
        /// </summary>
        /// <param name="onlyClientArea">Set this to <c>true</c> if only the client area should be redrawn.
        /// This avoids drawing the border and background.</param>
        public void Invalidate(bool onlyClientArea = false)
        {
            lock(Window.SynchronizationLock)
                if (onlyClientArea)
                    DrawClientArea(Window.GetGraphics());
                else
                    Draw();
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
        public void Draw(IConsoleGraphics? graphics = null)
        {
            Logger.Log(DebugContext.Control | DebugContext.Drawing, $"called {(graphics == null ? "without" : "with")} graphics.");
            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }

                var usedGraphics = graphics ?? Window.GetGraphics();
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing background.");
                DrawBackground(usedGraphics);
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing border.");
                DrawBorder(usedGraphics);
                Logger.Log(DebugContext.Control | DebugContext.Drawing, "Drawing client area.");
                DrawClientArea(usedGraphics);
                if (usedGraphics != graphics)
                {
                    Logger.Log(DebugContext.Control | DebugContext.Drawing, "Flushing graphics.");
                    usedGraphics.Flush();
                }
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
        /// Called when the <see cref="Name"/> has changed.
        /// </summary>
        protected virtual void OnNameChanged()
        {
            NameChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Called when the <see cref="Parent"/> has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
            using (DeferDrawing())
                ParentChanged?.Invoke(this, EventArgs.Empty);
        }
        void OnControlCollectionChanged(object sender, ControlCollectionChangedEventArgs e)
        {
            lock (Window.SynchronizationLock)
            {
                using(DeferDrawing())
                {
                    foreach (var addedControl in e.AddedControls)
                    {
                        addedControl.Parent = this;
                        addedControl.AreaChanged += OnControlAreaChanged;
                    }

                    foreach (var removedControl in e.RemovedControls)
                        removedControl.AreaChanged -= OnControlAreaChanged;
                }
            }
        }
        void OnControlAreaChanged(object sender, EventArgs e) => Invalidate(true);

        /// <summary>
        /// Called when the <see cref="CursorPosition"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnCursorPositionChanged()
        {
            using (DeferDrawing())
            {
                CursorPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="CursorPosition"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnCursorSizeChanged()
        {
            using (DeferDrawing())
            {
                CursorSizeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Called when the <see cref="CursorPosition"/> property of this control has been changed.
        /// </summary>
        protected virtual void OnCursorVisibleChanged()
        {
            using (DeferDrawing())
            {
                CursorVisibleChanged?.Invoke(this, EventArgs.Empty);
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
        /// <summary>
        /// Called when a <see cref="IConsoleWindow.KeyEvent"/> has been received.
        /// </summary>
        /// <param name="sender">The event source (must be <see cref="Window"/>).</param>
        /// <param name="e">The event details.</param>
        protected virtual void OnKeyEvent(object sender, KeyEventArgs e) { }
        void OnWindowKeyEvent(object sender, KeyEventArgs e)
        {
            lock (Window.SynchronizationLock)
                OnKeyEvent(sender, e);
        }
        /// <summary>
        /// Called when a <see cref="IConsoleWindow.MouseEvent"/> has been received.
        /// </summary>
        /// <param name="sender">The event source (must be <see cref="Window"/>).</param>
        /// <param name="e">The event details.</param>
        protected virtual void OnMouseEvent(object sender, MouseEventArgs e) { }
        void OnWindowMouseEvent(object sender, MouseEventArgs e)
        {
            lock (Window.SynchronizationLock)
                OnMouseEvent(sender, e);
        }
        /// <summary>
        /// Called when a <see cref="IConsoleWindow.StdOutEvent"/> has been received.
        /// </summary>
        /// <param name="sender">The event source (must be <see cref="Window"/>).</param>
        /// <param name="e">The event details.</param>
        protected virtual void OnStdOutEvent(object sender, StdOutEventArgs e) { }
        void OnWindowStdOutEvent(object sender, StdOutEventArgs e)
        {
            lock (Window.SynchronizationLock)
                OnStdOutEvent(sender, e);
        }
        /// <summary>
        /// Called when a <see cref="IConsoleWindow.StdErrEvent"/> has been received.
        /// </summary>
        /// <param name="sender">The event source (must be <see cref="Window"/>).</param>
        /// <param name="e">The event details.</param>
        protected virtual void OnStdErrEvent(object sender, StdErrEventArgs e) { }
        void OnWindowStdErrEvent(object sender, StdErrEventArgs e)
        {
            lock (Window.SynchronizationLock)
                OnStdErrEvent(sender, e);
        }
    }
}
