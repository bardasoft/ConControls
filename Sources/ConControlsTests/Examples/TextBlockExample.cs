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
using System.Linq;
using System.Threading.Tasks;
using ConControls.Controls;
using ConControls.Logging;
using ConControls.WindowsApi.Types;
// ReSharper disable AccessToDisposedClosure

// ReSharper disable AssignmentIsFullyDiscarded

namespace ConControlsTests.Examples
{
    [ExcludeFromCodeCoverage]
    class TextBlockExample : Example
    {
        public override DebugContext DebugContext => DebugContext.Text;
        public override async Task RunAsync()
        {
            const string text = @"[{0}] sample text with some longer
and
some shorter
lines.";
            using var window = new ConsoleWindow
            {
                Title = "ConControls: TextBlock example"
            };
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            using(window.DeferDrawing())
            {
                var panel = new Panel(window)
                {
                    BackgroundColor = ConsoleColor.DarkBlue,
                    Area = new Rectangle(1, 1, 50, 20),
                    BorderStyle = BorderStyle.Bold
                };
                window.Controls.Add(panel);
                bool active = true;
                window.KeyEvent += (sender, e) =>
                {
                    if (!e.KeyDown) return;
                    if (e.VirtualKey == VirtualKey.Escape)
                        tcs.SetResult(0);
                    if (e.VirtualKey == VirtualKey.F1)
                    {
                        active = !active;
                        window.SetActiveScreen(active);
                    }
                };

                var btClose = new Button(window)
                {
                    Area = new Rectangle(39, 15, 9, 3),
                    Text = "Close"
                };
                btClose.Click += (sender, e) => tcs.SetResult(0);
                var btAppend = new Button(window)
                {
                    Area = new Rectangle(0, 15, 10, 3),
                    Text = "Append"
                };
                int appends = 0;
                btAppend.Click += (sender, e) =>
                {
                    string txt = string.Format(text, ++appends);
                    foreach (var textBlock in panel.Controls.OfType<TextBlock>())
                        textBlock.Append(txt);
                };
                var btClear = new Button(window)
                {
                    Area = new Rectangle(10, 15, 9, 3),
                    Text = "Clear"
                };
                btClear.Click += (sender, e) =>
                {
                    foreach (var textBlock in panel.Controls.OfType<TextBlock>())
                        textBlock.Clear();
                };
                panel.Controls.AddRange(
                    new TextBlock(window)
                    {
                        Area = new Rectangle(0, 0, 10, 6),
                        BorderStyle = BorderStyle.None,
                        BackgroundColor = ConsoleColor.Blue,
                        ForegroundColor = ConsoleColor.White
                    },
                    new TextBlock(window)
                    {
                        Area = new Rectangle(12, 0, 10, 6),
                        BorderStyle = BorderStyle.None,
                        BackgroundColor = ConsoleColor.Blue,
                        ForegroundColor = ConsoleColor.White,
                        Wrap = true
                    },
                    new TextBlock(window)
                    {
                        Area = new Rectangle(0, 8, 10, 6),
                        BorderStyle = BorderStyle.SingleLined,
                        BackgroundColor = ConsoleColor.Blue,
                        ForegroundColor = ConsoleColor.White
                    },
                    new TextBlock(window)
                    {
                        Area = new Rectangle(12, 8, 10, 6),
                        BorderStyle = BorderStyle.SingleLined,
                        Wrap = true,
                        BackgroundColor = ConsoleColor.Blue,
                        ForegroundColor = ConsoleColor.White,
                        CursorVisible = false
                    },
                    btAppend,
                    btClear,
                    btClose
                );
                window.FocusedControl = panel.Controls[0];
            }

            await tcs.Task;
        }
    }
}
