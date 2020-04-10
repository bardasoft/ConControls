/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;

namespace ConControls.ConsoleApi
{
    [ExcludeFromCodeCoverage]
    sealed class ConsoleOutputReceivedEventArgs : EventArgs
    {
        public string Output
        {
            get;
        }
        public ConsoleOutputReceivedEventArgs(string output) => Output = output;
    }
}
