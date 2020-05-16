/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;

namespace ConControls.ConsoleApi
{
    interface IConsoleController : IDisposable
    {
        event EventHandler<ConsoleFocusEventArgs>? FocusEvent;
        event EventHandler<ConsoleKeyEventArgs>? KeyEvent;
        event EventHandler<ConsoleMouseEventArgs>? MouseEvent;
        event EventHandler<ConsoleSizeEventArgs>? SizeEvent;
        event EventHandler<ConsoleMenuEventArgs>? MenuEvent;
    }
}
