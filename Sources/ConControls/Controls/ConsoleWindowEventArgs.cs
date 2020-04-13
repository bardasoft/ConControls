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
    /// Abstract base class for <see cref="EventArgs"/> used by the events fired by <see cref="IConsoleWindow"/>.
    /// </summary>
    public abstract class ConsoleWindowEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets wether this event has been finally handled and
        /// no further action is needed.
        /// </summary>
        /// <remarks>
        /// When handling events of an <see cref="IConsoleWindow"/> like <seealso cref="IConsoleWindow.KeyEvent"/> or
        /// <seealso cref="IConsoleWindow.MouseEvent"/>, set this property to <c>true</c> if you handled that
        /// event and don't need the infrastructure tu take further care.<br/>
        /// If you don't handle the event, don't change this property.
        /// </remarks>
        [ExcludeFromCodeCoverage]
        public bool Handled { get; set; }

        private protected ConsoleWindowEventArgs() { }
    }
}
