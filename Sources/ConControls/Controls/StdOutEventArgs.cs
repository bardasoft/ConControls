/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.ConsoleApi;

namespace ConControls.Controls 
{
    /// <summary>
    /// Arguments for the <see cref="IConsoleWindow.StdOutEvent">IConsoleWindow.StdOutEvent</see>.
    /// </summary>
    public sealed class StdOutEventArgs : EventArgs
    {
        /// <summary>
        /// The received output converted into a string.
        /// </summary>
        public string Output { get; }
        internal StdOutEventArgs(ConsoleOutputReceivedEventArgs e)
        {
            Output = e.Output;
        }
    }
}
