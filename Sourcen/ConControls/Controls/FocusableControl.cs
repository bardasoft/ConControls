/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.Controls 
{
    /// <summary>
    /// A <see cref="ConsoleControl"/> that can take focus.
    /// </summary>
    public abstract class FocusableControl : ConsoleControl {
        /// <inheritdoc />
        private protected FocusableControl(IControlContainer parent)
            : base(parent) { }

        /// <inheritdoc />
        public override bool CanFocus() => true;
    }
}
