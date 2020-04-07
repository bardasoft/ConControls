/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using ConControls.WindowsApi.Types;

namespace ConControls.WindowsApi
{
    [ExcludeFromCodeCoverage]
    sealed class NativeCalls : INativeCalls
    {
        public CONSOLE_SCREEN_BUFFER_INFOEX GetConsoleScreenBufferInfo(ConsoleOutputHandle consoleOutputHandle)
        {
            CONSOLE_SCREEN_BUFFER_INFOEX info = new CONSOLE_SCREEN_BUFFER_INFOEX
            {
                Size = Marshal.SizeOf<CONSOLE_SCREEN_BUFFER_INFOEX>()
            };
            if (!NativeMethods.GetConsoleScreenBufferInfoEx(consoleOutputHandle, ref info))
                throw Exceptions.Win32();
            return info;
        }
        public string GetConsoleTitle()
        {
            StringBuilder titleBuilder = new StringBuilder(1024);
            if (NativeMethods.GetConsoleTitle(titleBuilder, 1024) <= 0)
                throw Exceptions.Win32();
            return titleBuilder.ToString();
        }
        public CHAR_INFO[] ReadConsoleOutput(ConsoleOutputHandle consoleOutputHandle, Rectangle region)
        {
            SMALL_RECT rect = new SMALL_RECT(region);
            CHAR_INFO[] buffer = new CHAR_INFO[region.Width * region.Height];
            if (!NativeMethods.ReadConsoleOutput(consoleOutputHandle, buffer, new COORD(region), default, ref rect))
                throw Exceptions.Win32();
            return buffer;
        }
        public void SetConsoleScreenBufferSize(ConsoleOutputHandle consoleOutputHandle, COORD size)
        {
            if (!NativeMethods.SetConsoleScreenBufferSize(consoleOutputHandle, size))
                throw Exceptions.Win32();
        }
        public void SetConsoleTitle(string title)
        {
            if (!NativeMethods.SetConsoleTitle(title))
                throw Exceptions.Win32();
        }
        public void WriteConsoleOutput(ConsoleOutputHandle consoleOutputHandle, CHAR_INFO[] buffer, Rectangle region)
        {
            SMALL_RECT rect = new SMALL_RECT(region);
            if (!NativeMethods.WriteConsoleOutput(consoleOutputHandle, buffer, new COORD(region), default, ref rect))
                throw Exceptions.Win32();
        }
    }
}
