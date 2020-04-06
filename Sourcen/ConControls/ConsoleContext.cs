using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using ConControls.WindowsApi;

namespace ConControls
{
    /// <summary>
    /// The context for a console UI session.
    /// </summary>
    /// <remarks>This context takes control over the console input buffer and
    /// the current screen buffer to provide UI functionality. Only one context
    /// may be instantiated at a time. Make sure to dispose of any previously
    /// instantiated contexts before creating a new one.
    /// </remarks>
    public sealed class ConsoleContext : IConsoleContext
    {
        static int instancesCreated;
        
        readonly INativeCalls api;
        readonly IntPtr consoleOutputHandle;

        int isDisposed;
        Size size;
        ConsoleColor backgroundColor = ConsoleColor.Black;

        /// <inheritdoc />
        public event EventHandler? SizeChanged;
        /// <inheritdoc />
        public event EventHandler? BackgroundColorChanged;
        /// <inheritdoc />
        public event EventHandler? Disposed;

        /// <inheritdoc />
        public Size Size
        {
            get
            {
                lock (SynchronizationLock) return size;
            }
            set
            {
                lock(SynchronizationLock)
                {
                    if (value == size) return;
                    if (value.Width < 1)
                        throw Exceptions.WidthTooSmall(nameof(Size), 1, value.Width);
                    if (value.Height < 1)
                        throw Exceptions.HeightTooSmall(nameof(Size), 1, value.Width);
                    size = value;
                    OnSizeChanged();
                }
            }
        }
        /// <inheritdoc />
        public int Width
        {
            get { lock(SynchronizationLock) return size.Width; }
            set
            {
                lock(SynchronizationLock)
                {
                    if (value == size.Width) return;
                    if (value < 1)
                        throw Exceptions.WidthTooSmall(nameof(Width), 1, value);
                    size = new Size(value, size.Height);
                    OnSizeChanged();
                }
            }
        }
        /// <inheritdoc />
        public int Height
        {
            get { lock(SynchronizationLock) return size.Height; }
            set
            {
                lock (SynchronizationLock)
                {
                    if (value == size.Height) return;
                    if (value < 1)
                        throw Exceptions.HeightTooSmall(nameof(Height), 1, value);
                    size = new Size(size.Width, value);
                    OnSizeChanged();
                }
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
        public string Title 
        {
            get => api.GetConsoleTitle();
            set => api.SetConsoleTitle(value ?? string.Empty);
        }

        /// <inheritdoc />
        public object SynchronizationLock { get; } = new object();
        /// <summary>
        /// This <see cref="ConsoleContext"/> instance has already been disposed.
        /// </summary>
        public bool IsDisposed => isDisposed != 0;

        /// <summary>
        /// Opens a new <see cref="ConsoleContext"/>. Only one instance can exist at a time.
        /// Be sure to dispose of any previously instantiated contexts.
        /// </summary>
        /// <exception cref="InvalidOperationException">A previously instantiated <see cref="ConsoleContext"/> has not yet been disposed of. Only a single context can exist at a time.</exception>
        [ExcludeFromCodeCoverage]
        public ConsoleContext()
            : this(null) { }
        internal ConsoleContext(INativeCalls? api)
        {
            if (Interlocked.CompareExchange(ref instancesCreated, 1, 0) != 0)
                throw Exceptions.CanOnlyUseSingleContext();
            this.api = api ?? new NativeCalls();
            consoleOutputHandle = this.api.GetStdHandle(NativeCalls.STDOUT);

            Refresh();
        }
        /// <summary>
        /// Cleans up native resources.
        /// </summary>
        ~ConsoleContext()
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
            Interlocked.Decrement(ref instancesCreated);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void Refresh() { }

        void OnSizeChanged()
        {
            Refresh();
            SizeChanged?.Invoke(this, EventArgs.Empty);
        }
        void OnBackgroundColorChanged()
        {
            Refresh();
            BackgroundColorChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
