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

namespace ConControls.Controls
{
    /// <summary>
    /// An abstract base class for console controls that display a scrollable block of text.
    /// </summary>
    public abstract class TextControl : ConsoleControl
    {
        string text = string.Empty;

        readonly IConsoleTextController textController;

        /// <summary>
        /// Raised when the <see cref="Text"/> property has been changed.
        /// </summary>
        public event EventHandler? TextChanged;

        /// <summary>
        /// Returns <c>true</c> for a <see cref="TextBlock"/>. This control can be focused.
        /// </summary>
        public override bool CanFocus => true;

        /// <summary>
        /// Gets or sets the text displayed in this <see cref="TextBlock"/> control.
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value == text) return;
                    text = value;
                    OnTextChanged();
                }
            }
        }

        /// <inheritdoc />
        private protected TextControl(IControlContainer parent)
            : this(parent, null) { }
        private protected TextControl(IControlContainer parent, IConsoleTextController? textController) : base(parent)
        {
            this.textController = textController ?? new ConsoleTextController();
            this.textController.BufferChanged += OnTextControllerBufferChanged;
            this.textController.CaretPositionChanged += OnTextControllerCaretPositionChanged;
            CursorSize = Window.CursorSize;
            CursorVisible = true;
            CursorPosition = new Point(0, 0);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            textController.CaretPositionChanged -= OnTextControllerCaretPositionChanged;
            textController.BufferChanged -= OnTextControllerCaretPositionChanged;
            base.Dispose(disposing);
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
                    Parent.PointToConsole(clientArea.Location),
                    textController.Buffer,
                    textController.Size);
            }
        }

        /// <summary>
        /// Called when the <see cref="Text"/> property has been changed.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        void OnTextControllerCaretPositionChanged(object sender, EventArgs e)
        {
            var rect = GetClientArea();
            CursorPosition = Point.Add(rect.Location, (Size)textController.CaretPosition);
        }
        void OnTextControllerBufferChanged(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
