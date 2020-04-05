/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ConControls.WindowsApi;

namespace ConControlsTests
{
    static class ConControlsTestsCli
    {
        static void Main()
        {
            var task = Task.Run(ReadEvents);
            task.Wait();
        }
        static void ReadEvents()
        {
            INativeCalls api = new NativeCalls();
            IntPtr stdInHandle = api.GetStdHandle(NativeCalls.STDIN);
            
            if (stdInHandle.ToInt64() <= 0)
            {
                Log($"FAILED TO GET STDHANDLE: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }
            if (!api.GetConsoleMode(stdInHandle, out ConsoleInputModes modes))
            {
                Log($"FAILED TO GET MODE: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }
            Log($"CURRENT MODE: {modes}");
            modes = ConsoleInputModes.EnableExtendedFlags | ConsoleInputModes.EnableMouseInput | ConsoleInputModes.EnableWindowInput;
            if (!api.SetConsoleMode(stdInHandle, modes))
            {
                Log($"FAILED TO SET MODE: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }
            if (!api.GetConsoleMode(stdInHandle, out modes))
            {
                Log($"FAILED TO GET UPDATED MODE: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                return;
            }
            Log($"UPDATED MODE: {modes}");
            while (true)
            {
                var buffer = new INPUT_RECORD[128];
                if (!api.ReadConsoleInput(stdInHandle, buffer, (uint)buffer.Length, out var read))
                {
                    Log($"FAILED TO READ INPUT: {new Win32Exception(Marshal.GetLastWin32Error()).Message}");
                    return;
                }

                Log($"READ {read} records:");
                foreach (var record in buffer.Take((int)read))
                {
                    Log("===================");
                    Log($"EVENT: {record.EventType}");
                    switch (record.EventType)
                    {
                        case InputEventType.Key:
                            Log($"Key: {record.Event.KeyEvent.VirtualKeyCode} ({(record.Event.KeyEvent.KeyDown ? "down" : "up")})");
                            Log($"Control: {record.Event.KeyEvent.ControlKeys}");
                            break;
                        case InputEventType.Mouse:
                            Debug.WriteLine($"Position: {record.Event.MouseEvent.MousePosition.X}, {record.Event.MouseEvent.MousePosition.Y}");
                            if (record.Event.MouseEvent.EventFlags.HasFlag(MouseEventFlags.Wheeled) | record.Event.MouseEvent.EventFlags.HasFlag(MouseEventFlags.WheeledHorizontally))
                                Log($"Scroll: {record.Event.MouseEvent.Scroll}");
                            else
                                Log($"Buttons: {record.Event.MouseEvent.ButtonState}");
                            Log($"Event: {record.Event.MouseEvent.EventFlags}");
                            Log($"Control: {record.Event.MouseEvent.ControlKeys}");
                            break;
                        case InputEventType.WindowBufferSize:
                            Log($"Size: {record.Event.SizeEvent.Size.X}, {record.Event.SizeEvent.Size.Y}");
                            break;
                        case InputEventType.Menu:
                            Log($"Menu: {record.Event.MenuEent.CommandId}");
                            break;
                        case InputEventType.Focus:
                            Log($"Focus: {record.Event.FocusEvent.SetFocus}");
                            break;
                        default:
                            Log("Unknown");
                            break;
                    }
                }
            }
        }
        static void Log(string msg)
        {
            Debug.WriteLine(msg);
        }
    }
}
