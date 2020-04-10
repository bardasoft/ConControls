using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using ConControls;
using ConControls.Controls;

namespace ConControlsTests.Examples
{
    static class ProgressBarExample
    {
        public static void Run()
        {
            using var window = new ConsoleWindow();
            Debug.WriteLine($"SIZE: {window.Size} MAX: {window.MaximumSize}");
            window.BackgroundColor = ConsoleColor.Blue;
            var l2r = new ConsoleProgressBar(window)
            {
                Name = "l2r",
                Area = new Rectangle(0, 0, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.LeftToRight
            };
            var r2l = new ConsoleProgressBar(window)
            {
                Name = "r2l",
                Area = new Rectangle(3, 10, 10, 3),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.RightToLeft
            };
            var t2b = new ConsoleProgressBar(window)
            {
                Name = "t2b",
                Area = new Rectangle(0, 3, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.TopToBottom
            };
            var b2t = new ConsoleProgressBar(window)
            {
                Name = "b2t",
                Area = new Rectangle(10, 0, 3, 10),
                BackgroundColor = ConsoleColor.Cyan,
                BorderColor = ConsoleColor.Yellow,
                BorderStyle = BorderStyle.Bold,
                ForeColor = ConsoleColor.Green,
                Orientation = ConsoleProgressBar.ProgressOrientation.BottomToTop
            };

            for (int i = 0; i < 2000000; i++)
            {
                double p = (double)(i % 101) / 100;
                window.BeginUpdate();
                l2r.Percentage = r2l.Percentage = t2b.Percentage = b2t.Percentage = p;
                window.EndUpdate();
                Thread.Sleep(200);
            }
        }
    }
}
