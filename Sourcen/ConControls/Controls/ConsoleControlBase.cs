using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using ConControls.ConsoleApi;

namespace ConControls.Controls
{
    /// <summary>
    /// Base class for all console controls.
    /// </summary>
    public abstract class ConsoleControlBase
    {
        Rectangle effectiveBounds;
        ConsoleControlBase? parent;
        int inhibitDrawing;
        BorderStyle? borderStyle;
        ConsoleColor? borderColor;
        ConsoleColor? backgroundColor;

        /// <summary>
        /// The <see cref="Area"/> of the control has been changed.
        /// </summary>
        public event EventHandler? AreaChanged;

        /// <summary>
        /// The effective total area of the control.
        /// This is the area the control effectivly fills in the console screen buffer
        /// after applying layout and including borders.
        /// </summary>
        public Rectangle Area
        {
            get
            {
                lock (Window.SynchronizationLock) return effectiveBounds;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == effectiveBounds) return;
                    effectiveBounds = value;
                    OnAreaChanged();
                }
            }
        }
        /// <summary>
        /// The <see cref="IConsoleWindow"/> that contains this control.
        /// </summary>
        public IConsoleWindow Window { get; }
        /// <summary>
        /// The parent <see cref="ConsoleControlBase"/> that contains this control.
        /// The parent must be contained by the same <see cref="IConsoleWindow"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The parent is not part of the same <see cref="IConsoleWindow"/> or this control is the root element of the window..</exception>
        /// <exception cref="ArgumentNullException">The parent is <code>null</code>.</exception>
        public ConsoleControlBase? Parent
        {
            get => parent;
            set
            {
                if (Window.Panel == this) throw Exceptions.CannotChangeRootPanelsParent();
                if (value == null) throw new ArgumentNullException(nameof(Parent));

                lock(Window.SynchronizationLock)
                {
                    if (parent == value) return;
                    if (value.Window != Window) throw Exceptions.DifferentWindow();
                    parent?.Controls.Remove(this);
                    parent = value;
                    parent.Controls.Add(this);
                    OnParentChanged();
                }
            }

        }
        /// <summary>
        /// The collection of <see cref="ConsoleControlBase"/>s contained by this control.
        /// </summary>
        public ControlCollection Controls { get; }

        /// <summary>
        /// Determines if the control can currently be redrawn, depending on calls to <see cref="BeginUpdate"/> and
        /// <see cref="EndUpdate"/> to this control or its parents.
        /// </summary>
        public bool DrawingInhibited => inhibitDrawing > 0 || parent?.DrawingInhibited == true || Window.DrawingInhibited;

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

        private protected ConsoleControlBase(IConsoleWindow window)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
            Controls = new ControlCollection(this);
            Controls.ControlAdded += OnControlAdded;
            Controls.ControlRemoved += OnControlRemoved;
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

        void Draw()
        {
            Debug.WriteLine("ConsoleControlBase.Draw: called.");
            lock (Window.SynchronizationLock)
            {
                if (DrawingInhibited)
                {
                    Debug.WriteLine("ConsoleControlBase.Draw: drawing inhibited.");
                    return;
                }
                Debug.WriteLine("ConsoleControlBase.Draw: executing.");
                var graphics = Window.GetGraphics();
                Draw(graphics);
                graphics.Flush();
            }
        }
        /// <summary>
        /// Draws the control onto the console screen buffer.
        /// When overwriting this method, make sure to use the <see cref="IConsoleWindow.SynchronizationLock"/>
        /// to synchronize threads and to call <see cref="CheckDisposed"/> to check if the window has not yet
        /// been disposed of.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        public virtual void Draw(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));
            Debug.WriteLine("ConsoleControlBase.Draw(IConsoleGrahics): called.");

            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (DrawingInhibited)
                {
                    Debug.WriteLine("ConsoleControlBase.Draw(IConsoleGrahics): drawing inhibited.");
                    return;
                }

                var effectiveBackgroundColor = EffectiveBackgroundColor;
                var effectiveBorderColor = EffectiveBorderColor;
                var effectiveBorderStyle = EffectiveBorderStyle;

                Debug.WriteLine($"ConsoleControlBase.Draw(IConsoleGrahics): drawing background ({effectiveBackgroundColor}.");
                graphics.DrawBackground(backgroundColor ?? parent?.backgroundColor ?? Window.BackgroundColor, effectiveBounds);
                Debug.WriteLine($"ConsoleControlBase.Draw(IConsoleGrahics): drawing border ({effectiveBorderColor}, {effectiveBorderStyle}.");
                graphics.DrawBorder(effectiveBackgroundColor, effectiveBorderColor, effectiveBorderStyle, effectiveBounds);
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
        /// Called when the <see cref="Parent"/> has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
        }
        /// <summary>
        /// Called when a <see cref="ConsoleControlBase"/> has been added to this control.
        /// </summary>
        /// <param name="sender">The object that raised the event (must be <see cref="Controls"/>).</param>
        /// <param name="e">The <see cref="ControlCollectionChangedEventArgs"/> containing the added <see cref="ConsoleControlBase"/>.</param>
        protected virtual void OnControlAdded(object sender, ControlCollectionChangedEventArgs e)
        {
            Draw();
        }
        /// <summary>
        /// Called when a <see cref="ConsoleControlBase"/> has been removed from this control.
        /// </summary>
        /// <param name="sender">The object that raised the event (must be <see cref="Controls"/>).</param>
        /// <param name="e">The <see cref="ControlCollectionChangedEventArgs"/> containing the removed <see cref="ConsoleControlBase"/>.</param>
        protected virtual void OnControlRemoved(object sender, ControlCollectionChangedEventArgs e)
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
