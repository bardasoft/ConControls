using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using ConControls.ConsoleApi;
using ConControls.Controls;
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
        readonly ConsoleInputHandle consoleInputHandle;
        readonly ConsoleOutputHandle consoleOutputHandle;

        int isDisposed;
        int inhibitDrawing;
        Size size;
        ConsoleColor backgroundColor = ConsoleColor.Black;

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
                    size = new Size(size.Width, value);
                    OnSizeChanged();
                }
            }
        }

        /// <inheritdoc />
        public ConsoleControl Panel { get; }

        /// <inheritdoc />
        public bool DrawingInhibited => inhibitDrawing > 0;

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
            : this(null) { }
        internal ConsoleWindow(INativeCalls? api)
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
            this.api.SetConsoleMode(consoleInputHandle,
                                    ConsoleInputModes.EnableWindowInput | 
                                    ConsoleInputModes.EnableMouseInput |
                                    ConsoleInputModes.EnableExtendedFlags);
            this.api.SetConsoleMode(consoleOutputHandle, ConsoleOutputModes.None);
            Panel = new RootPanel(this);
            Panel.AreaChanged += (sender, e) => AdjustWindowAndBufferSize();
            AdjustWindowAndBufferSize();
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
                consoleOutputHandle.Dispose();
                consoleInputHandle.Dispose();
            }
            Interlocked.Decrement(ref instancesCreated);
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public IConsoleGraphics GetGraphics() => new ConsoleGraphics(consoleOutputHandle, api, Size);
        /// <inheritdoc />
        public void Draw()
        {
            Debug.WriteLine("ConsoleWindow.Draw: called.");
            lock (SynchronizationLock)
            {
                if (DrawingInhibited)
                {
                    Debug.WriteLine("ConsoleWindow.Draw: drawing inhibited.");
                    return;
                }
                var graphics = GetGraphics();
                var rect = new Rectangle(0, 0, Size.Width, Size.Height);
                Debug.WriteLine($"ConsoleWindow.Draw: drawing background at {rect}.");
                graphics.DrawBackground(backgroundColor, rect);
                Debug.WriteLine("ConsoleWindow.Draw: drawing root panel.");
                Panel.Draw(graphics);
                Debug.WriteLine("ConsoleWindow.Draw: flushing.");
                graphics.Flush();
            }
        }
        /// <inheritdoc />
        public void Refresh() => AdjustWindowAndBufferSize();
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
        void OnSizeChanged()
        {
            Draw();
        }
        void OnBackgroundColorChanged()
        {
            Draw();
        }

        void AdjustWindowAndBufferSize()
        {
            lock (SynchronizationLock)
            {
                BeginUpdate();
                try
                {
                    var info = api.GetConsoleScreenBufferInfo(consoleOutputHandle);
                    Debug.WriteLine(
                        $"ConsoleWindow.AdjustWindowAndBufferSize: Read window props ({info.Window.Left}, {info.Window.Top}, {info.Window.Right}, {info.Window.Bottom}).");
                    var winSize = new Size(info.Window.Right - info.Window.Left + 1, info.Window.Bottom - info.Window.Top + 1);
                    Debug.WriteLine($"ConsoleWindow.AdjustWindowAndBufferSize: Calculated window size {winSize}).");
                    api.SetConsoleScreenBufferSize(consoleOutputHandle, new COORD(winSize));
                    Size = winSize;
                    var panelBounds = Panel.Area;
                    int left = panelBounds.Left;
                    if (left >= Size.Width)
                        left = Size.Width - 10;
                    int right = left + panelBounds.Width;
                    if (right >= Size.Width)
                        right = Size.Width - 1;
                    int top = panelBounds.Top;
                    if (top >= Size.Height)
                        top = Size.Height - 10;
                    int bottom = top + panelBounds.Height;
                    if (bottom >= Size.Height)
                        bottom = Size.Height - 1;
                    Rectangle panelRect = new Rectangle(left, top, right - left, bottom - top);
                    Debug.WriteLine($"ConsoleWindow.AdjustWindowAndBufferSize: Setting root panel to area {panelRect}.");
                    Panel.Area = panelRect;
                }
                finally
                {
                    EndUpdate();
                }
            }
        }
    }
}
