/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.ConsoleApi
{
    sealed class ConsoleOutputReceivedEventArgs : EventArgs
    {
        public string Output
        {
            get;
        }
        public ConsoleOutputReceivedEventArgs(string output) => Output = output;
    }
}
