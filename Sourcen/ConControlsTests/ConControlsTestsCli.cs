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
using System.Threading;
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
            Debug.WriteLine($"SIZE: {window.Size} MAX: {window.MaximumSize}");
            window.BackgroundColor = ConsoleColor.Blue;
            var l2r = new ConsoleProgressBar(window)
            {
                Name = "myprogressbar",
                Area = new Rectangle(0, 0, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.LeftToRight
            };
            var r2l = new ConsoleProgressBar(window)
            {
                Name = "myprogressbar",
                Area = new Rectangle(3, 10, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.RightToLeft
            };
            var t2b = new ConsoleProgressBar(window)
            {
                Name = "myprogressbar",
                Area = new Rectangle(0, 3, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.TopToBottom
            };
            var b2t = new ConsoleProgressBar(window)
            {
                Name = "myprogressbar",
                Area = new Rectangle(10, 0, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.BottomToTop
            };

            for (int i = 0; i < 2000000; i++)
            {
                double p = (double)(i % 101)/100;
                window.BeginUpdate();
                l2r.Percentage = r2l.Percentage = t2b.Percentage = b2t.Percentage = p;
                window.EndUpdate();
                Thread.Sleep(200);
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
