﻿/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Threading.Tasks;
using ConControls.Controls;
using ConControls.WindowsApi.Types;

// ReSharper disable AccessToDisposedClosure

// ReSharper disable AssignmentIsFullyDiscarded

namespace ConControlsExamples.Examples
{
    class WindowSizeExample : Example
    {
        public override async Task RunAsync()
        {
            using var window = new ConsoleWindow
            {
                Title = "ConControls: WindowSize example"
            };
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
            using(window.DeferDrawing())
            {
                const int smallX = 50, largeX = 150;
                const int smallY = 20, largeY = 60;
                Size
                    small = new Size(smallX, smallY),
                    large = new Size(largeX, largeY),
                    longer = new Size(smallX, largeY),
                    wider = new Size(largeX, smallY);

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

                var panel = new Panel(window)
                {
                    Parent = window,
                    BorderStyle = BorderStyle.DoubleLined,
                    Area = new Rectangle(Point.Empty, window.Size)
                };
                var btLarge = new Button(window)
                {
                    Area = new Rectangle(0, 0, 11, 3),
                    Text = " Large "
                };
                var btSmall = new Button(window)
                {
                    Area = new Rectangle(0, 3, 11, 3),
                    Text = " Small "
                };
                var btWide= new Button(window)
                {
                    Area = new Rectangle(0, 6, 11, 3),
                    Text = " Wide  "
                };
                var btLong = new Button(window)
                {
                    Area = new Rectangle(0, 9, 11, 3),
                    Text = " Long  "
                };

                btLarge.Click += (sender, e) => SetSize(large);
                btSmall.Click += (sender, e) => SetSize(small);
                btWide.Click += (sender, e) => SetSize(wider);
                btLong.Click += (sender, e) => SetSize(longer);
                
                panel.Controls.AddRange(btLarge, btSmall, btWide, btLong);

                void SetSize(Size s)
                {
                    using(window.DeferDrawing())
                    {
                        window.Size = s;
                        panel.Area = new Rectangle(Point.Empty, s);
                    }
                }

            }

            await tcs.Task;
        }
    }
}
