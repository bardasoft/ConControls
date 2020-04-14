/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading;
using ConControls.Controls;
using ConControls.WindowsApi.Types;

namespace ConControlsTests.Examples
{
    [ExcludeFromCodeCoverage]
    static class ProgressBarExample
    {
        public static void Run()
        {
            using var window = new ConsoleWindow
            {
                BackgroundColor = ConsoleColor.Blue
            };
            bool stop = false;
            window.KeyEvent += (sender, e) =>
            {
                if (e.KeyDown && e.VirtualKey == VirtualKey.Escape)
                    stop = true;
            };

            var l2r = new ProgressBar(window)
            {
                Name = "l2r",
                Area = new Rectangle(0, 0, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForegroundColor = ConsoleColor.Green,
                Orientation = ProgressBar.ProgressOrientation.LeftToRight
            };
            var r2l = new ProgressBar(window)
            {
                Name = "r2l",
                Area = new Rectangle(3, 10, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForegroundColor = ConsoleColor.Green,
                Orientation = ProgressBar.ProgressOrientation.RightToLeft
            };
            var t2b = new ProgressBar(window)
            {
                Name = "t2b",
                Area = new Rectangle(0, 3, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForegroundColor = ConsoleColor.Green,
                Orientation = ProgressBar.ProgressOrientation.TopToBottom
            };
            var b2t = new ProgressBar(window)
            {
                Name = "b2t",
                Area = new Rectangle(10, 0, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForegroundColor = ConsoleColor.Green,
                Orientation = ProgressBar.ProgressOrientation.BottomToTop
            };

            for (int i = 0; i < 2000000 && !stop; i++)
            {
                double p = (double)(i % 101) / 100;
                using (window.DeferDrawing())
                    l2r.Percentage = r2l.Percentage = t2b.Percentage = b2t.Percentage = p;
                if (i % 10 == 0)
                    Console.WriteLine($"Output at {i}");
                Thread.Sleep(50);
            }
        }
    }
}
