/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using ConControls.Controls.Drawing;
using ConControls.Controls.Text;
using ConControls.Logging;
using ConControls.WindowsApi.Types;

namespace ConControls.Controls
{
    /// <summary>
    /// An abstract base class for console controls that display a scrollable block of text.
    /// </summary>
    public abstract class TextControl : ConsoleControl
    {
        readonly IConsoleTextController textController;

        bool caretVisible = true, initCursorVisibility = true;
        Point scroll = Point.Empty;
        Point caret;

        /// <summary>
        /// Raised when the <see cref="Text"/> property has been changed.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Returns <c>true</c> for a <see cref="TextBlock"/>. This control can be focused.
        /// </summary>
        public override bool CanFocus => true;

        /// <inheritdoc />
        public override bool CursorVisible
        {
            get => (base.CursorVisible || initCursorVisibility) && caretVisible;
            set
            {
                initCursorVisibility = false;
                base.CursorVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in this <see cref="TextBlock"/> control.
        /// </summary>
        public virtual string Text
        {
            get
            {
                lock (Window.SynchronizationLock) return textController.Text;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == textController.Text) return;
                    textController.Text = value;
                    OnTextChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the caret position.
        /// </summary>
        public Point Caret
        {
            get
            {
                lock (Window.SynchronizationLock) return caret;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    var tmp = textController.ValidateCaret(value);
                    if (value == caret) return;
                    caret = tmp;
                    UpdateCursorPosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets wether lines are wrapped or not.
        /// </summary>
        public bool Wrap
        {
            get
            {
                lock(Window.SynchronizationLock) return textController.Wrap;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == textController.Wrap) return;
                    textController.Wrap= value;
                }
            }
        }

        /// <inheritdoc />
        private protected TextControl(IConsoleWindow window)
            : this(window, null) { }
        private protected TextControl(IConsoleWindow window, IConsoleTextController? textController)
            : base(window) =>
            this.textController = textController ?? new ConsoleTextController();

        /// <inheritdoc />
        protected override void OnAreaChanged()
        {
            lock (Window.SynchronizationLock)
            {
                textController.Width = GetClientArea().Width;
                UpdateCursorPosition();
            }

            base.OnAreaChanged();
        }
        /// <inheritdoc />
        protected override void OnBorderStyleChanged()
        {
            lock (Window.SynchronizationLock)
            {
                textController.Width = GetClientArea().Width;
                UpdateCursorPosition();
            }

            base.OnBorderStyleChanged();
        }

        /// <inheritdoc />
        protected override void OnKeyEvent(object sender, KeyEventArgs e)
        {
            _ = e ?? throw new ArgumentNullException(nameof(e));
            if (!(Focused && Enabled && Visible && e.KeyDown) || e.Handled)
            {
                base.OnKeyEvent(sender, e);
                return;
            }

            switch (e.VirtualKey)
            {
                case VirtualKey.Up:
                    MoveCaretUp();
                    e.Handled = true;
                    break;
                case VirtualKey.Down:
                    MoveCaretDown();
                    e.Handled = true;
                    break;
                case VirtualKey.Left:
                    MoveCaretLeft();
                    e.Handled = true;
                    break;
                case VirtualKey.Right:
                    MoveCaretRight();
                    e.Handled = true;
                    break;
                case VirtualKey.Home:
                    MoveCaretHome(e.ControlKeys.HasFlag(ControlKeyStates.LEFT_CTRL_PRESSED) | e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_CTRL_PRESSED));
                    e.Handled = true;
                    break;
                case VirtualKey.End:
                    MoveCaretEnd(e.ControlKeys.HasFlag(ControlKeyStates.LEFT_CTRL_PRESSED) | e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_CTRL_PRESSED));
                    e.Handled = true;
                    break;
                case VirtualKey.Prior:
                    MoveCaretPageUp();
                    e.Handled = true;
                    break;
                case VirtualKey.Next:
                    MoveCaretPageDown();
                    e.Handled = true;
                    break;
            }

            base.OnKeyEvent(sender, e);
        }
        /// <inheritdoc />
        protected override void OnMouseEvent(object sender, MouseEventArgs e)
        {
            _ = e ?? throw new ArgumentNullException(nameof(e));

            if (e.Handled || !(Enabled && Visible))
            {
                base.OnMouseEvent(sender, e);
                return;
            }

            if (e.ButtonState == MouseButtonStates.LeftButtonPressed)
            {
                var clientArea = GetClientArea();
                var clientPoint = PointToClient(e.Position);
                if (clientArea.Contains(clientPoint))
                {
                    e.Handled = true;
                    Caret = Point.Add(Point.Subtract(clientPoint, (Size)clientArea.Location), (Size)scroll);
                    Focused = true;
                }
            }

            base.OnMouseEvent(sender, e);
        }

        /// <inheritdoc />
        protected override void DrawClientArea(IConsoleGraphics graphics)
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
                var effectiveForegroundColor = EffectiveForegroundColor;

                Logger.Log(DebugContext.Control | DebugContext.Drawing, $"drawing text ({effectiveForegroundColor} on {effectiveBackgroundColor}).");
                var clientArea = GetClientArea();
                graphics.CopyCharacters(
                    effectiveBackgroundColor,
                    effectiveForegroundColor,
                    PointToConsole(clientArea.Location),
                    textController.GetCharacters(new Rectangle(scroll.X, scroll.Y, clientArea.Width, clientArea.Height)),
                    clientArea.Size);
            }
        }

        /// <summary>
        /// Called when the <see cref="Text"/> property has been changed.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        void UpdateCursorPosition()
        {
            Point caretPosition = Point.Subtract(caret, (Size)scroll);
            var clientArea = GetClientArea();
            var cursorPos = Point.Add(caretPosition, (Size)clientArea.Location);
            if (clientArea.Contains(cursorPos))
            {
                CursorPosition = Point.Add(caretPosition, (Size)clientArea.Location);
                if (!caretVisible)
                {
                    caretVisible = true;
                    OnCursorVisibleChanged();
                }

            }
            else if (caretVisible)
            {
                caretVisible = false;
                OnCursorVisibleChanged();
            }
        }
        void MoveCaretUp()
        {
            Caret = new Point(caret.X, caret.Y - 1);
            ScrollToCaret();
        }
        void MoveCaretDown()
        {
            Caret = new Point(caret.X, caret.Y + 1);
            ScrollToCaret();
        }
        void MoveCaretLeft()
        {
            if (caret == Point.Empty) return;
            if (caret.X == 0)
                Caret = new Point(textController.GetLineLength(caret.Y - 1), caret.Y - 1);
            else
                Caret = new Point(caret.X - 1, caret.Y);
            ScrollToCaret();
        }
        void MoveCaretRight()
        {
            int length = textController.GetLineLength(caret.Y);
            if (caret.X == length)
                Caret = new Point(0, caret.Y + 1);
            else
                Caret = new Point(caret.X + 1, caret.Y);
            ScrollToCaret();
        }
        void MoveCaretHome(bool ctrl)
        {
            Caret = new Point(0, ctrl ? 0 : caret.Y);
            ScrollToCaret();
        }
        void MoveCaretEnd(bool ctrl)
        {
            int y = ctrl ? textController.BufferLineCount - 1 : caret.Y;
            Caret = new Point(textController.GetLineLength(y), y);
            ScrollToCaret();
        }
        void MoveCaretPageUp()
        {
            var clientArea = GetClientArea();
            Caret = new Point(caret.X, caret.Y - clientArea.Height);
            ScrollToCaret();
        }
        void MoveCaretPageDown()
        {
            var clientArea = GetClientArea();
            Caret = new Point(caret.X, caret.Y + clientArea.Height);
            ScrollToCaret();
        }
        void ScrollToCaret()
        {
            var clientArea = GetClientArea();
            int x = scroll.X;
            if (caret.X - x < 0)
                x = caret.X;
            else if (caret.X - x >= clientArea.Width)
                x = caret.X - clientArea.Width + 1;
            int y = scroll.Y;
            if (caret.Y - y < 0)
                y = caret.Y;
            else if (caret.Y - y >= clientArea.Height)
                y = caret.Y - clientArea.Height + 1;

            var nextScroll = new Point(x, y);
            if (nextScroll != scroll)
            {
                scroll = nextScroll;
                UpdateCursorPosition();
                Invalidate();
            }
        }
    }
}
