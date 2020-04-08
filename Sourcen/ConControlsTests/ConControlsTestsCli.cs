/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls;
using ConControls.Controls;

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
// ReSharper disable UnusedMember.Local

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static void RunTest()
        {
            using var window = new ConsoleWindow();
            window.BeginUpdate();
            try
            {
                window.BackgroundColor = ConsoleColor.Blue;
                window.Panel.Area = new Rectangle(0, 0, window.Width, window.Height);
                window.Panel.BackgroundColor = ConsoleColor.Cyan;
                window.Panel.BorderColor = ConsoleColor.Yellow;
                window.Panel.BorderStyle = BorderStyle.Bold;
            }
            finally
            {
                window.EndUpdate();
            }

            Console.ReadLine();
        }

        static void Main()
        {
            //Task.Run(ReadEvents).Wait();
            using var logger = new Logger(@"c:\privat\concontrols.log");
            try
            {
                RunTest();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            
            Debug.WriteLine("Done.");

            //ConsoleColor[] colors = Enum.GetValues(typeof(ConsoleColor)).Cast<ConsoleColor>().ToArray();
            //foreach (var color in colors)
            //{
            //    Console.ForegroundColor = color;
            //    Console.BackgroundColor = color;
            //    Console.WriteLine(' ');
            //}

            //var api = new NativeCalls();
            //CHAR_INFO[] buffer = api.ReadConsoleOutput(new ConsoleOutputHandle(), new Rectangle(0, 0, 1, colors.Length));
            //Console.ResetColor();
            //Console.Clear();
            //File.AppendAllLines(@"c:\Privat\colors.txt",colors.Select((color, i) =>
            //                                                $"{color} = {buffer[i].Attributes}"));
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
