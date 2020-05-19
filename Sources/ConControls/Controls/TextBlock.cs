/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace ConControls.Controls
{
    /// <summary>
    /// A console control to display a scrollable block of text.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class TextBlock : TextControl
    {
        /// <inheritdoc />
        public TextBlock(IConsoleWindow parent)
            : base(parent)
        { }
    }
}
