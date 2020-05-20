/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace ConControls.Controls
{
    /// <summary>
    /// A console control to display a scrollable block of text.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class TextBlock : TextControl
    {
        /// <summary>
        /// Creates a new <see cref="TextBlock"/> instance.
        /// </summary>
        /// <param name="window">The parent <see cref="IConsoleWindow"/> this control should belong to.</param>
        /// <exception cref="ArgumentNullException"><paramref name="window"/> is <c>null</c>.</exception>
        public TextBlock(IConsoleWindow window)
            : base(window)
        { }
    }
}
