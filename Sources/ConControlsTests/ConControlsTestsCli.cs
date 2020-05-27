/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable
#pragma warning disable IDE0051

// ReSharper disable UnusedMember.Local

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Threading.Tasks;
using ConControls.Controls;
using ConControls.Logging;
using ConControlsTests.UnitTests;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static async Task Main()
        {
            using var consoleLogger = new TestLogger(Console.WriteLine);
            Logger.Context = DebugContext.Window;

            Console.WriteLine("Starting test.");
            try
            {
                using var window = new ConsoleWindow
                {
                    SwitchConsoleBuffersKey = KeyCombination.F11,
                    CloseWindowKey = KeyCombination.AltF4,
                    BackgroundColor = ConsoleColor.DarkBlue,
                    Title = "ConControls example"
                };
                using(window.DeferDrawing())
                {
                    var frame = new Panel(window)
                    {
                        Parent = window,
                        Area = window.Area,
                        BorderStyle = BorderStyle.DoubleLined,
                    };
                    _ = new TextBlock(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(2, 1, frame.Size.Width - 6, 3),
                        BorderStyle = BorderStyle.SingleLined,
                        BackgroundColor = ConsoleColor.Red,
                        ForegroundColor = ConsoleColor.Yellow,
                        Text = "Welcome to ConControls!"
                    };
                    _ = new TextBlock(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(2, 6, 9, 1),
                        Text = "Progress:"
                    };
                    _ = new ProgressBar(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(12, 5, 45, 3),
                        BackgroundColor = ConsoleColor.Blue,
                        ForegroundColor = ConsoleColor.Green,
                        BorderStyle = BorderStyle.SingleLined,
                        Percentage = 0.4
                    };
                    _ = new TextBlock(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(2, 11, 6, 1),
                        Text = "Output:"
                    };
                    _ = new TextBlock(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(12, 10, 45, 10),
                        BorderStyle = BorderStyle.Bold,
                        BackgroundColor = ConsoleColor.Black,
                        ForegroundColor = ConsoleColor.Gray,
                        Text = "Your output goes here...\nLike debug messages or nice\nlittle stories..."
                    };
                    var button = new Button(window)
                    {
                        Parent = frame,
                        Area = new Rectangle(48, 21, 9, 3),
                        Text = "Close"
                    };
                    button.Click += (sender, e) => window.Close();
                }

                await window.WaitForCloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Test finished.");
        }
    }
}
