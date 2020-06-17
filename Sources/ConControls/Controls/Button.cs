/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.Controls.Text;
using ConControls.Helpers;
using ConControls.WindowsApi.Types;

namespace ConControls.Controls {
    /// <summary>
    /// A button control.
    /// </summary>
    public sealed class Button : TextControl
    {
        bool initialTabStop = true, initialCanFocus = true;

        /// <summary>
        /// Raised when the button gets clicked.
        /// </summary>
        public event EventHandler? Click;

        /// <inheritdoc />
        public override bool TabStop
        {
            get => base.TabStop || initialTabStop;
            set
            {
                initialTabStop = false;
                base.TabStop = value;
            }
        }
        /// <inheritdoc />
        public override bool CanFocus
        {
            get => base.CanFocus || initialCanFocus;
            set
            {
                initialCanFocus = false;
                base.CanFocus = value;
            }
        }

        /// <inheritdoc />
        public override string Text
        {
            get
            {
                lock (Window.SynchronizationLock)
                {
                    string s = base.Text;
                    if (s.Length < 2) return string.Empty;
                    return s.Substring(1, s.Length - 2);
                }
            }
            set => base.Text = $"[{value}]";
        }
        /// <inheritdoc />
        public override bool CursorVisible
        {
            get => false;
            set => base.CursorVisible = value;
        }

        /// <summary>
        /// Creates a new instance of a <see cref="Button"/>.
        /// </summary>
        /// <param name="window">The <see cref="IConsoleWindow"/> this control should belong to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <c>null</c>.</exception>
        public Button(IConsoleWindow window)
            : this(window, null) { }

        internal Button(IConsoleWindow window, IConsoleTextController? textController)
            : base(window, textController)
        {
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.DarkYellow;
            DisabledBackgroundColor = ConsoleColor.DarkBlue;
            DisabledForegroundColor = ConsoleColor.Gray;
            BorderColor = ConsoleColor.DarkYellow;
            BorderStyle = BorderStyle.SingleLined;
            FocusedBackgroundColor = ConsoleColor.Blue;
            FocusedForegroundColor = ConsoleColor.Yellow;
            FocusedBorderColor = ConsoleColor.Yellow;
            FocusedBorderStyle = BorderStyle.DoubleLined;
        }

        /// <summary>
        /// Performs a button click (if the control is <see cref="ConsoleControl.Enabled"/>).
        /// </summary>
        public void PerformClick()
        {
            lock (Window.SynchronizationLock) if (Enabled && Visible) OnClick();
        }

        void OnClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        protected override void OnKeyEvent(KeyEventArgs e)
        {
            base.OnKeyEvent(e);
            if (e.Handled || 
                !e.KeyDown ||
                e.ControlKeys.WithoutSwitches() != ControlKeyStates.None ||
                e.VirtualKey != VirtualKey.Return && e.VirtualKey != VirtualKey.Space)
                return;

            PerformClick();
            e.Handled = true;
        }

        /// <inheritdoc />
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            // When this class is no longer sealed,
            // we need to check for Enabled and Visible properties.
            if (e.ButtonState != MouseButtonStates.LeftButtonPressed)
                return;
            
            e.Handled = true;
            PerformClick();
        }
    }
}
