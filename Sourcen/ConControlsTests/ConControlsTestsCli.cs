/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ConControls.Logging;
using ConControlsTests.Examples;

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
// ReSharper disable UnusedMember.Local

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static void RunTest()
        {
            ProgressBarExample.Run();
            Console.ReadLine();
        }

        static void Main()
        {
            //Task.Run(ReadEvents).Wait();
            using var logger = new Logger(@"c:\privat\concontrols.log");
            ConControls.Logging.Logger.Context = DebugContext.ProgressBar;

            try
            {
                RunTest();
                //Task.Run(ReadEvents).Wait();
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
        //    var handle = new ConsoleInputHandle();
        //    INPUT_RECORD[] buffer = new INPUT_RECORD[16];
        //    api.SetConsoleMode(handle, ConsoleInputModes.EnableWindowInput | ConsoleInputModes.EnableMouseInput | ConsoleInputModes.EnableExtendedFlags);
        //    //while (NativeMethods.ReadConsoleInput(handle, buffer, buffer.Length, out var read))
        //    //    foreach (var sizeRecord in buffer.Take(read))
        //    //        Console.WriteLine($"Event: {sizeRecord.EventType}");
        //    while (true)

        //    {
        //        var records = api.ReadConsoleInput(handle);
        //        foreach (var sizeRecord in records)
        //            Console.WriteLine($"Event: {sizeRecord.EventType}");
        //    }
        //}
    }
}
