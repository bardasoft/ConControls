/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
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

        bool caretVisible, canFocus;
        Point scroll;
        Point caret;

        /// <summary>
        /// Raised when the <see cref="Text"/> property has been changed.
        /// </summary>
        public event EventHandler? TextChanged;
        /// <summary>
        /// Raised when the <see cref="CanFocus"/> property has been changed.
        /// </summary>
        public event EventHandler? CanFocusChanged;

        /// <summary>
        /// Gets or sets wether this <see cref="TextControl"/> is read-only or can be edited.
        /// </summary>
        /// <exception cref="NotSupportedException">Setting this property is not supported by the current control's type.</exception>
        public virtual bool CanEdit
        {
            get => false;
            set => throw Exceptions.PropertySetterNotSupported(GetType().Name, nameof(CanEdit));
        }

        /// <summary>
        /// Returns <c>true</c> for a <see cref="TextBlock"/>. This control can be focused.
        /// </summary>
        public override bool CanFocus
        {
            get
            {
                lock (Window.SynchronizationLock) return canFocus;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (canFocus == value) return;
                    canFocus = value;
                    OnCanFocusChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the scroll dimension. The <see cref="Point.X"/> part of this <see cref="Point"/> defines
        /// the number of character columns scrolled to the right, and the <see cref="Point.Y"/> part defines the
        /// number of character rows scrolled down.
        /// </summary>
        public virtual Point Scroll
        {
            get
            {
                lock (Window.SynchronizationLock) return scroll;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == scroll) return;
                    scroll = value;
                    UpdateCursorPosition();
                    Invalidate();
                }
            }
        }

        /// <inheritdoc />
        public override bool CursorVisible
        {
            get => base.CursorVisible && caretVisible;
            set => base.CursorVisible = value;
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
                _ = value ?? throw new ArgumentNullException(paramName: nameof(value));
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
                    if (tmp == caret) return;
                    caret = tmp;
                    UpdateCursorPosition();
                }
            }
        }

        /// <summary>
        /// Gets or sets wether lines are wrapped or not.
        /// </summary>
        public virtual WrapMode WrapMode
        {
            get
            {
                lock(Window.SynchronizationLock) return textController.WrapMode;
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == textController.WrapMode) return;
                    textController.WrapMode = value;
                }
            }
        }

        /// <inheritdoc />
        [ExcludeFromCodeCoverage]
        private protected TextControl(IConsoleWindow window)
            : this(window, null) { }
        [ExcludeFromCodeCoverage]
        private protected TextControl(IConsoleWindow window, IConsoleTextController? textController)
            : base(window) =>
            this.textController = textController ?? new ConsoleTextController();

        /// <summary>
        /// Appends the given <paramref name="text"/> to the current text.
        /// If the caret is positioned at the end of the current text, it will
        /// be placed at the end of the appended text and <see cref="ScrollToCaret"/>
        /// will be called after to make sure the appended text is shown.
        /// </summary>
        /// <param name="text">The string to append to the current text.</param>
        /// <exception cref="ArgumentNullException"><paramref name="text"/> is <c>null</c>.</exception>
        public void Append(string text)
        {
            _ = text ?? throw new ArgumentNullException(paramName: nameof(text));
            lock (Window.SynchronizationLock)
            {
                using(DeferDrawing())
                {
                    bool caretAtEnd = caret == textController.EndCaret;
                    textController.Append(text);
                    if (!caretAtEnd) return;
                    Caret = textController.EndCaret;
                    ScrollToCaret();
                }
            }
        }
        /// <inheritdoc />
        public override void Clear()
        {
            lock (Window.SynchronizationLock)
            {
                using(DeferDrawing())
                {
                    textController.Clear();
                    Caret = Point.Empty;
                    Scroll = Point.Empty;
                }
            }
            base.Clear();
        }
        /// <summary>
        /// Scrolls the content so that the caret (cursor) becomes visible.
        /// If the caret already is inside the displayed text, the scrolling will not be changed.
        /// If the caret is outside the displayed text, scrolling is adjusted so that the caret
        /// just appears in the client area.
        /// </summary>
        public void ScrollToCaret()
        {
            lock (Window.SynchronizationLock)
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

                Scroll = new Point(x, y);
            }
        }

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
        protected override void OnCursorPositionChanged()
        {
            base.OnCursorPositionChanged();
            Caret = Point.Add(CursorPosition, (Size)scroll);
        }
        /// <inheritdoc />
        protected override void OnKeyEvent(KeyEventArgs e)
        {
            _ = e ?? throw new ArgumentNullException(nameof(e));
            if (!(Focused && Enabled && Visible && e.KeyDown) || e.Handled)
            {
                base.OnKeyEvent(e);
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
                    MoveCaretHome(e.ControlKeys.HasFlag(ControlKeyStates.LEFT_CTRL_PRESSED) || e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_CTRL_PRESSED));
                    e.Handled = true;
                    break;
                case VirtualKey.End:
                    MoveCaretEnd(e.ControlKeys.HasFlag(ControlKeyStates.LEFT_CTRL_PRESSED) || e.ControlKeys.HasFlag(ControlKeyStates.RIGHT_CTRL_PRESSED));
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

            base.OnKeyEvent(e);
        }
        /// <summary>
        /// This method is called when the <see cref="TextControl"/> has been clicked on.
        /// It sets the cursor position to the clicked location. If there is not enough
        /// content to set the cursor to this position, the neares possible cursor position will be used.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> containing the details of the event.</param>
        /// <remarks>
        /// The <see cref="MouseEventArgs.Position"/> property of the arguments <paramref name="e"/> contains the mouse position in client coordinates (relative to the control's client area).
        /// </remarks>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            _ = e ?? throw new ArgumentNullException(nameof(e));
            base.OnMouseClick(e);

            if (e.Handled || !(Enabled && Visible) || e.ButtonState != MouseButtonStates.LeftButtonPressed) return;
            e.Handled = true;
            Caret = Point.Add(e.Position, (Size)scroll);
            Focused = true;
        }
        /// <summary>
        /// This method is called when the mouse wheel has been used above this <see cref="TextControl"/>.
        /// It sets scrolls the content if necessary and possible.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> containing the details of the event.</param>
        /// <remarks>
        /// The <see cref="MouseEventArgs.Position"/> property of the arguments <paramref name="e"/> contains the mouse position in client coordinates (relative to the control's client area).
        /// </remarks>
        protected override void OnMouseScroll(MouseEventArgs e)
        {
            _ = e ?? throw new ArgumentNullException(nameof(e));
            base.OnMouseScroll(e);

            if (e.Handled || !(Enabled && Visible)) return;

            switch (e.Kind)
            {
                case MouseEventFlags.Wheeled:
                    ScrollVertically(e.Scroll);
                    e.Handled = true;
                    break;
                case MouseEventFlags.WheeledHorizontally:
                    ScrollHoritzontically(e.Scroll);
                    e.Handled = true;
                    break;
            }
        }
        /// <summary>
        /// Called when the <see cref="CanFocus"/> property changed.<br/>
        /// Raises the <see cref="CanFocusChanged"/> event.
        /// </summary>
        protected virtual void OnCanFocusChanged() => CanFocusChanged?.Invoke(this, EventArgs.Empty);

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
                    PointToConsole(Point.Empty),
                    textController.GetCharacters(new Rectangle(scroll.X, scroll.Y, clientArea.Width, clientArea.Height)),
                    clientArea.Size);
            }
        }

        /// <summary>
        /// Called when the <see cref="Text"/> property has been changed.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            using (DeferDrawing())
                TextChanged?.Invoke(this, EventArgs.Empty);
        }

        void UpdateCursorPosition()
        {
            Point caretPosition = Point.Subtract(caret, (Size)scroll);
            var clientArea = new Rectangle(Point.Empty, GetClientArea().Size);
            if (clientArea.Contains(caretPosition))
            {
                CursorPosition = caretPosition;
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
        void ScrollVertically(int delta)
        {
            int y = scroll.Y - delta / 120;
            int maxLineIndex = Math.Max(0, textController.BufferLineCount - 1);
            y = Math.Max(0, y);
            y = Math.Min(maxLineIndex, y);
            Scroll = new Point(scroll.X, y);
        }
        void ScrollHoritzontically(int delta)
        {
            int x = scroll.X - delta / 120;
            x = Math.Max(0, x);
            x = Math.Min(textController.MaxLineLength - 1, x);
            Scroll = new Point(x, scroll.Y);
        }
        void MoveCaretUp()
        {
            Caret = textController.MoveCaretUp(caret);
            ScrollToCaret();
        }
        void MoveCaretDown()
        {
            Caret = textController.MoveCaretDown(caret);
            ScrollToCaret();
        }
        void MoveCaretLeft()
        {
            Caret = textController.MoveCaretLeft(caret);
            ScrollToCaret();
        }
        void MoveCaretRight()
        {
            Caret = textController.MoveCaretRight(caret);
            ScrollToCaret();
        }
        void MoveCaretHome(bool ctrl)
        {
            Caret = ctrl ? textController.MoveCaretHome(caret) : textController.MoveCaretToBeginOfLine(caret);
            ScrollToCaret();
        }
        void MoveCaretEnd(bool ctrl)
        {
            Caret = ctrl ? textController.MoveCaretEnd(caret) : textController.MoveCaretToEndOfLIne(caret);
            ScrollToCaret();
        }
        void MoveCaretPageUp()
        {
            var clientArea = GetClientArea();
            Caret = textController.MoveCaretPageUp(caret, clientArea.Height);
            ScrollToCaret();
        }
        void MoveCaretPageDown()
        {
            var clientArea = GetClientArea();
            Caret = textController.MoveCaretPageDown(caret, clientArea.Height);
            ScrollToCaret();
        }
    }
}
