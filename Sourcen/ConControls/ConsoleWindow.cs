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
using ConControls.Controls;
using ConControls.Logging;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;

namespace ConControls
{
    /// <summary>
    /// The window for a console UI session.
    /// </summary>
    /// <remarks>This window takes control over the console input buffer and
    /// the current screen buffer to provide UI functionality. Only one window
    /// may be instantiated at a time. Make sure to dispose of any previously
    /// instantiated contexts before creating a new one.
    /// </remarks>
    public sealed class ConsoleWindow : IConsoleWindow
    {
        static int instancesCreated;
        
        readonly INativeCalls api;
        readonly IConsoleListener consoleListener;
        readonly ConsoleInputHandle consoleInputHandle;
        readonly ConsoleOutputHandle consoleOutputHandle;

        int isDisposed;
        int inhibitDrawing;
        Size lastKnownSize;

        FrameCharSets frameCharSets = new FrameCharSets();
        ConsoleColor foreColor = ConsoleColor.Gray;
        ConsoleColor backgroundColor = ConsoleColor.Black;

        ConsoleControl? focusedControl;

        /// <inheritdoc />
        public event EventHandler? SizeChanged;
        /// <inheritdoc />
        public event EventHandler? Disposed;

        /// <inheritdoc />
        public string Title
        {
            get => api.GetConsoleTitle();
            set => api.SetConsoleTitle(value ?? string.Empty);
        }
        /// <inheritdoc />
        public Size Size
        {
            get
            {
                var info = api.GetConsoleScreenBufferInfo(consoleOutputHandle);
                return new Size(info.Window.Right - info.Window.Left + 1, info.Window.Bottom - info.Window.Top + 1);
            }
            set
            {
                lock (SynchronizationLock)
                {
                    if (value == Size) return;
                    api.SetConsoleWindowSize(consoleOutputHandle, value);
                    SynchronizeConsoleSettings();
                }
            }
        }
        /// <inheritdoc />
        public Size MaximumSize
        {
            get
            {
                var size = api.GetLargestConsoleWindowSize(consoleOutputHandle);
                return new Size(size.X, size.Y);
            }
        }
        /// <inheritdoc />
        public ConsoleColor ForeColor
        {
            get => foreColor;
            set
            {
                if (value == foreColor) return;
                foreColor = value;
                OnForeColorChanged();
            }
        }
        /// <inheritdoc />
        public ConsoleColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                if (value == backgroundColor) return;
                backgroundColor = value;
                OnBackgroundColorChanged();
            }
        }
        /// <inheritdoc />
        public ControlCollection Controls { get; }
        /// <inheritdoc />
        public ConsoleControl? FocusedControl 
        {
            get
            {
                lock (SynchronizationLock) return focusedControl;
            }
            set
            {
                lock (SynchronizationLock)
                {
                    if (value == focusedControl || value == null) return;
                    if (focusedControl != null) focusedControl.Focused = false;
                    focusedControl = null;
                    value.Focused = true; // may throw
                    focusedControl = value;
                }
            }
        }

        /// <inheritdoc />
        public FrameCharSets FrameCharSets
        {
            get => frameCharSets;
            set
            {
                if (frameCharSets == value) return;
                if (value == null) throw new ArgumentNullException(nameof(FrameCharSets));

                lock (SynchronizationLock)
                {
                    frameCharSets = value;
                    OnFrameCharSetsChanged();
                }
            }
        }
        /// <inheritdoc />
        public bool DrawingInhibited => inhibitDrawing > 0;
        /// <inheritdoc />
        public bool IsDisposed => isDisposed != 0;
        /// <inheritdoc />
        public object SynchronizationLock { get; } = new object();

        /// <summary>
        /// Opens a new <see cref="ConsoleWindow"/>. Only one instance can exist at a time.
        /// Be sure to dispose of any previously instantiated contexts.
        /// </summary>
        /// <exception cref="InvalidOperationException">A previously instantiated <see cref="ConsoleWindow"/> has not yet been disposed of. Only a single window can exist at a time.</exception>
        [ExcludeFromCodeCoverage]
        public ConsoleWindow()
            : this(null, null) { }
        internal ConsoleWindow(INativeCalls? api, IConsoleListener? consoleListener)
        {
            if (Interlocked.CompareExchange(ref instancesCreated, 1, 0) != 0)
                throw Exceptions.CanOnlyUseSingleContext();

            consoleInputHandle = new ConsoleInputHandle();
            if (consoleInputHandle.IsInvalid)
                throw Exceptions.Win32();
            consoleOutputHandle = new ConsoleOutputHandle();
            if (consoleOutputHandle.IsInvalid)
                throw Exceptions.Win32();
            this.api = api ?? new NativeCalls();
            this.consoleListener = consoleListener ?? new ConsoleListener(this.api);
            Controls = new ControlCollection(this);
            Controls.ControlAdded += (sender, e) => Draw();
            Controls.ControlRemoved += (sender, e) => Draw();
            SynchronizeConsoleSettings();
        }
        /// <summary>
        /// Cleans up native resources.
        /// </summary>
        ~ConsoleWindow()
        {
            Dispose(false);
        }
        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        void Dispose(bool disposing)
        {
            if (Interlocked.CompareExchange(ref isDisposed, 1, 0) != 0) return;
            if (disposing)
            {
                consoleListener.Dispose();
                consoleOutputHandle.Dispose();
                consoleInputHandle.Dispose();
                Console.ResetColor();
                Console.Clear();
            }
            Interlocked.Decrement(ref instancesCreated);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public IConsoleGraphics GetGraphics() => new ConsoleGraphics(consoleOutputHandle, api, Size, frameCharSets);
        /// <inheritdoc />
        public void Draw()
        {
            Logger.Log(DebugContext.Window | DebugContext.Drawing, "called.");
            lock (SynchronizationLock)
            {
                if (DrawingInhibited)
                {
                    Logger.Log(DebugContext.Window | DebugContext.Drawing, "drawing inhibited.");
                    return;
                }
                var graphics = GetGraphics();
                var rect = new Rectangle(0, 0, Size.Width, Size.Height);
                Logger.Log(DebugContext.Window | DebugContext.Drawing, $"drawing background at {rect}.");
                graphics.DrawBackground(backgroundColor, rect);
                Logger.Log(DebugContext.Window | DebugContext.Drawing, "drawing controls.");
                foreach(var control in Controls)
                    control.Draw(graphics);
                Logger.Log(DebugContext.Window | DebugContext.Drawing, "flushing.");
                graphics.Flush();
            }
        }
        /// <inheritdoc />
        public void Refresh() => SynchronizeConsoleSettings();
        /// <inheritdoc />
        public void BeginUpdate()
        {
            Interlocked.Increment(ref inhibitDrawing);
        }
        /// <inheritdoc />
        public void EndUpdate()
        {
            if (Interlocked.Decrement(ref inhibitDrawing) <= 0)
                Draw();
        }
        void OnFrameCharSetsChanged()
        {
            Draw();
        }
        void OnSizeChanged()
        {
            BeginUpdate();
            try
            {
                SizeChanged?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                EndUpdate();
            }
        }
        void OnForeColorChanged()
        {
            Draw();
        }
        void OnBackgroundColorChanged()
        {
            Draw();
        }

        void SynchronizeConsoleSettings()
        {
            lock (SynchronizationLock)
            {
                BeginUpdate();
                try
                {
                    var size = Size;
                    if (size != lastKnownSize)
                    {
                        Logger.Log(DebugContext.Window, "Window size has changed since last synchronization.");
                        lastKnownSize = size;
                        OnSizeChanged();
                    }

                    var bufferSize = new COORD(size);
                    Logger.Log(DebugContext.Window, $"Setting screen buffer size to {bufferSize}.");
                    api.SetConsoleScreenBufferSize(consoleOutputHandle, new COORD(Size));
                }
                finally
                {
                    EndUpdate();
                }
            }
        }
    }
}
