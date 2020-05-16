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
    /// A console panel that can contain other console controls.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ConsolePanel : ConsoleControl {
        /// <inheritdoc />
        public ConsolePanel(IControlContainer parent)
            : base(parent) { }
    }
}
