/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.WindowsApi;

namespace ConControls.ConsoleApi
{
    interface IConsoleListener : IDisposable
    {
        event EventHandler<ConsoleOutputReceivedEventArgs>? OutputReceived;
        event EventHandler<ConsoleFocusEventArgs>? FocusEvent;
        event EventHandler<ConsoleKeyEventArgs>? KeyEvent;
        event EventHandler<ConsoleMouseEventArgs>? MouseEvent;
        event EventHandler<ConsoleSizeEventArgs>? SizeEvent;
        event EventHandler<ConsoleMenuEventArgs>? MenuEvent;

        ConsoleErrorHandle OriginalErrorHandle { get; }
        ConsoleInputHandle OriginalInputHandle { get; }
        ConsoleOutputHandle OriginalOutputHandle { get; }
    }
}
