/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ConControls.Controls;
using ConControls.Controls.Text;

// ReSharper disable AccessToDisposedClosure

// ReSharper disable AssignmentIsFullyDiscarded

namespace ConControlsExamples.Examples
{
    class TextBlockExample : Example
    {
        public override async Task RunAsync()
        {
            const string text = @"[{0}] sample text with some longer
and
some shorter
lines.";
            using var window = new ConsoleWindow
            {
                Title = "ConControls: TextBlock example",
                CloseWindowKey = KeyCombination.Escape,
                SwitchConsoleBuffersKey = KeyCombination.F11

            };
            using(window.DeferDrawing())
            {
                var panel = new Panel(window)
                {
                    BackgroundColor = ConsoleColor.DarkBlue,
                    Area = new Rectangle(1, 1, 50, 20),
                    BorderStyle = BorderStyle.Bold
                };
                window.Controls.Add(panel);

                var btClose = new Button(window)
                {
                    Area = new Rectangle(39, 15, 9, 3),
                    Text = "Close"
                };
                btClose.Click += (sender, e) => window.Close();
                var block1 = new TextBlock(window)
                {
                    Area = new Rectangle(0, 0, 10, 6),
                    BorderStyle = BorderStyle.None,
                    BackgroundColor = ConsoleColor.Blue,
                    ForegroundColor = ConsoleColor.White
                };
                var block2 = new TextBlock(window)
                {
                    Area = new Rectangle(12, 0, 10, 6),
                    BorderStyle = BorderStyle.None,
                    BackgroundColor = ConsoleColor.Blue,
                    ForegroundColor = ConsoleColor.White,
                    WrapMode = WrapMode.SimpleWrap
                };
                var block3 = new TextBlock(window)
                {
                    Area = new Rectangle(0, 8, 10, 6),
                    BorderStyle = BorderStyle.SingleLined,
                    BackgroundColor = ConsoleColor.Blue,
                    ForegroundColor = ConsoleColor.White
                };
                var label = new Label(window)
                {
                    Area = new Rectangle(12, 8, 10, 6),
                    BorderStyle = BorderStyle.SingleLined,
                    WrapMode = WrapMode.SimpleWrap,
                    BackgroundColor = ConsoleColor.Blue,
                    ForegroundColor = ConsoleColor.White
                };
                var btClear = new Button(window)
                {
                    Area = new Rectangle(10, 15, 9, 3),
                    Text = "Clear"
                };
                var btAppend = new Button(window)
                {
                    Area = new Rectangle(0, 15, 10, 3),
                    Text = "Append"
                };
                int appends = 0;
                btAppend.Click += (sender, e) =>
                {
                    string txt = string.Format(text, ++appends);
                    foreach (var textBlock in panel.Controls.OfType<TextControl>())
                        textBlock.Append(txt);
                };
                btClear.Click += (sender, e) =>
                {
                    block1.Clear();
                    block2.Clear();
                    block3.Clear();
                    label.Clear();
                };
                panel.Controls.AddRange(
                          block1,
                          block2,
                          block3,
                          label,
                          btAppend,
                          btClear,
                          btClose
                      );
                window.FocusedControl = panel.Controls[0];
            }

            await window.WaitForCloseAsync();
        }
    }
}
