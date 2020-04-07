using System;
using System.Drawing;
using System.Threading;
using ConControls.ConsoleApi;
using ConControls.WindowsApi;

namespace ConControls.Controls
{
    /// <summary>
    /// Base class for all console controls.
    /// </summary>
    public abstract class ConsoleControl
    {
        readonly INativeCalls api;

        Rectangle effectiveBounds;
        ConsoleControl? parent;
        int inhibitUpdate;
        ConsoleColor? backgroundColor;

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
        /// The collection of <see cref="ConsoleControl"/>s contained by this control.
        /// </summary>
        public ControlCollection Controls { get; }

        /// <summary>
        /// Determines if the control can currently be redrawn, depending on calls to <see cref="BeginUpdate"/> and
        /// <see cref="EndUpdate"/> to this control or its parents.
        /// </summary>
        public bool UpdateInhibited => inhibitUpdate > 0 || parent?.UpdateInhibited == true || Window.UpdateInhibited;

        /// <summary>
        /// The background color of this control.
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

        private protected ConsoleControl(IConsoleWindow window) : this(window, null) { }
        private protected ConsoleControl(IConsoleWindow window, INativeCalls? api)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
            this.api = api ?? new NativeCalls();
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

        /// <summary>
        /// Redraws the control into the console screen buffer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The containing <see cref="IConsoleWindow"/> has already been disposed of.</exception>
        public virtual void Draw(IConsoleGraphics graphics)
        {
            if (graphics == null) throw new ArgumentNullException(nameof(graphics));

            lock (Window.SynchronizationLock)
            {
                CheckDisposed();
                if (UpdateInhibited) return;
                graphics.DrawBackground(backgroundColor ?? parent?.backgroundColor ?? Window.BackgroundColor, effectiveBounds);
            }
        }
        void Draw()
        {
            var graphics = Window.GetGraphics();
            Draw(graphics);
            graphics.Flush();
        }

        /// <summary>
        /// Inhibits any redrawing etc. until <see cref="EndUpdate"/> is called.
        /// Use this to avoid multiple redrawings while updating multiple properties.
        /// </summary>
        public void BeginUpdate()
        {
            Interlocked.Increment(ref inhibitUpdate);
        }
        /// <summary>
        /// Finishes an update sequence. Call <see cref="BeginUpdate"/> before you
        /// update multiple properties to avoid multiple redrawings, and call <see cref="EndUpdate"/>
        /// when you are finished and want to redraw the control.
        /// </summary>
        public void EndUpdate()
        {
            if (Interlocked.Decrement(ref inhibitUpdate) <= 0)
                Draw();
        }

        /// <summary>
        /// Called when the <see cref="Parent"/> has changed.
        /// </summary>
        protected virtual void OnParentChanged()
        {
        }
        /// <summary>
        /// Called when a <see cref="ConsoleControl"/> has been added to this control.
        /// </summary>
        /// <param name="sender">The object that raised the event (must be <see cref="Controls"/>).</param>
        /// <param name="e">The <see cref="ControlCollectionChangedEventArgs"/> containing the added <see cref="ConsoleControl"/>.</param>
        protected virtual void OnControlAdded(object sender, ControlCollectionChangedEventArgs e)
        {
            Draw();
        }
        /// <summary>
        /// Called when a <see cref="ConsoleControl"/> has been removed from this control.
        /// </summary>
        /// <param name="sender">The object that raised the event (must be <see cref="Controls"/>).</param>
        /// <param name="e">The <see cref="ControlCollectionChangedEventArgs"/> containing the removed <see cref="ConsoleControl"/>.</param>
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
            if (Parent == null)
                Window.Draw();
            else
                Parent.Draw();
        }
    }
}
