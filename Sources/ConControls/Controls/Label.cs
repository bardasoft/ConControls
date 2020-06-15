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
    /// A console control to display a text label.
    /// </summary>
    /// <remarks>A label is not focusable and can not be a tab stop. The text can be scrolled by mouse though, if it does not fit in the control's area.</remarks>
    public sealed class Label : TextControl
    {
        /// <summary>
        /// Creates a new <see cref="Label"/> instance.
        /// </summary>
        /// <param name="window">The <see cref="IConsoleWindow"/> this control should belong to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <c>null</c>.</exception>
        [ExcludeFromCodeCoverage]
        public Label(IConsoleWindow window)
            : base(window) { }
        [ExcludeFromCodeCoverage]
        internal Label(IConsoleWindow window, IConsoleTextController? textController)
            : base(window, textController) { }
    }
}
