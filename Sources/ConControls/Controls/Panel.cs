/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.Controls 
{
    /// <summary>
    /// A console panel control. A plain container
    /// for other controls.
    /// </summary>
    public sealed class Panel : ConsoleControl
    {
        /// <inheritdoc />
        public Panel(IControlContainer parent) : base(parent) { }
    }
}
