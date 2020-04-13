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
    /// Arguments for the <see cref="IConsoleWindow.StdErrEvent">IConsoleWindow.StdErrEvent</see>.
    /// </summary>
    public sealed class StdErrEventArgs : EventArgs
    {
        /// <summary>
        /// The received error output converted into a string.
        /// </summary>
        public string Error { get; }
        internal StdErrEventArgs(ConsoleOutputReceivedEventArgs e)
        {
            Error = e.Output;
        }
    }
}
