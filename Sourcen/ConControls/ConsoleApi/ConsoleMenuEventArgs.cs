/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi {
    sealed class ConsoleMenuEventArgs : EventArgs
    {
        public uint CommandId { get; }
        public ConsoleMenuEventArgs(MENU_EVENT_RECORD rec)
        {
            CommandId = rec.CommandId;
        }
    }
}
