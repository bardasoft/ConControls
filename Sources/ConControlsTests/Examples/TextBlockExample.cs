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
using System.Threading.Tasks;
using ConControls.Controls;
using ConControls.Logging;
using ConControls.WindowsApi.Types;
// ReSharper disable AssignmentIsFullyDiscarded

namespace ConControlsTests.Examples
{
    [ExcludeFromCodeCoverage]
    class TextBlockExample : Example
    {
        public override DebugContext DebugContext => DebugContext.Text;
        public override async Task RunAsync()
        {
            using var window = new ConsoleWindow
            {
                Title = "ConControls: TextBlock example"
            };
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            window.KeyEvent += (sender, e) =>
            {
                if (e.KeyDown && e.VirtualKey == VirtualKey.Escape)
                    tcs.SetResult(0);
            };

            using(window.DeferDrawing())
            {
                var panel = new Panel(window)
                {
                    BackgroundColor = ConsoleColor.DarkBlue,
                    Area = new Rectangle(1, 1, 50, 20),
                    BorderStyle = BorderStyle.Bold
                };

                _ = new TextBlock(panel)
                {
                    Area = new Rectangle(2, 2, 20, 8),
                    BorderStyle = BorderStyle.DoubleLined,
                    BackgroundColor = ConsoleColor.Blue,
                    ForegroundColor = ConsoleColor.White,
                    Text = $"First block of text.{Environment.NewLine}New line of text."
                };
                _ = new TextBlock(panel)
                {
                    Area = new Rectangle(25, 2, 10, 6),
                    BorderStyle = BorderStyle.SingleLined,
                    BackgroundColor = ConsoleColor.DarkGreen,
                    ForegroundColor = ConsoleColor.White,
                    Text = $"Second block of text.{Environment.NewLine}New line of text."
                };
                _ = new TextBlock(panel)
                {
                    Area = new Rectangle(2, 11, 46, 7),
                    BorderStyle = BorderStyle.SingleLined,
                    BorderColor = ConsoleColor.Blue,
                    BackgroundColor = ConsoleColor.White,
                    ForegroundColor = ConsoleColor.Black,
                    Text = $"Disabled bottom block with a longer line and ...{Environment.NewLine}New line of text.",
                    Enabled = false
                };

                window.FocusedControl = panel.Controls[0];
            }

            await tcs.Task;
        }
    }
}
