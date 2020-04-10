/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi 
{
    [ExcludeFromCodeCoverage]
    sealed class ConsoleFocusEventArgs : EventArgs
    {
        public bool SetFocus { get; }
        public ConsoleFocusEventArgs(FOCUS_EVENT_RECORD rec) => SetFocus = rec.SetFocus != 0;
    }
}
