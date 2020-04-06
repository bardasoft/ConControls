/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ConControls;
using ConControls.WindowsApi;
using ConControls.WindowsApi.Types;

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
// ReSharper disable UnusedMember.Local

namespace ConControlsTests
{
    static class ConControlsTestsCli
    {
        static void Main()
        {
            //Task.Run(ReadEvents).Wait();
            using var context = new ConsoleContext();
            Console.WriteLine(context.Title);
            context.Title = "hallo welt!";
            Console.WriteLine(context.Title);
        }
        //static void ReadEvents()
        //{
        //    var api = new NativeCalls();
        //    var handle = api.GetStdHandle(NativeCalls.STDIN);
        //    INPUT_RECORD[] buffer = new INPUT_RECORD[16];
        //    api.SetConsoleMode(handle, ConsoleInputModes.EnableWindowInput | ConsoleInputModes.EnableMouseInput | ConsoleInputModes.EnableExtendedFlags);
        //    while (api.ReadConsoleInput(handle, buffer, buffer.Length, out var read))
        //        foreach (var sizeRecord in buffer.Take(read).Where(r => r.EventType == InputEventType.WindowBufferSize))
        //            Console.WriteLine($"Size: {sizeRecord.Event.SizeEvent.Size.X} {sizeRecord.Event.SizeEvent.Size.Y}");
        //}
        //static string GetLastError() => new Win32Exception(Marshal.GetLastWin32Error()).Message;
    }
}
