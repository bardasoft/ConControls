/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.Controls.Text;

namespace ConControls.Controls 
{
    /// <summary>
    /// A console control to display a block of text.
    /// </summary>
    public sealed class TextBlock : TextControl
    {
        bool initTabStop = true, initCanFocus = true, initCursorVisible = true;

        /// <inheritdoc />
        public override bool TabStop
        {
            get => base.TabStop || initTabStop;
            set
            {
                initTabStop = false;
                base.TabStop = value;
            }
        }
        /// <inheritdoc />
        public override bool CanFocus
        {
            get => base.CanFocus || initCanFocus;
            set
            {
                initCanFocus = false;
                base.CanFocus = value;
            }
        }
        /// <inheritdoc />
        public override bool CursorVisible
        {
            get => base.CursorVisible || initCursorVisible;
            set
            {
                initCursorVisible = false;
                base.CursorVisible = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="TextBlock"/> instance.
        /// </summary>
        /// <param name="window">The <see cref="IConsoleWindow"/> this control should belong to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <c>null</c>.</exception>
        [ExcludeFromCodeCoverage]
        public TextBlock(IConsoleWindow window)
            : base(window) { }
        internal TextBlock(IConsoleWindow window, IConsoleTextController? textController)
            : base(window, textController) { }
    }
}
