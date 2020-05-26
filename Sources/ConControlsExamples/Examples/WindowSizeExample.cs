/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using System.Threading.Tasks;
using ConControls.Controls;

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
                Title = "ConControls: WindowSize example",
                CloseWindowKey = KeyCombination.Escape,
                SwitchConsoleBuffersKey = KeyCombination.F11
            };
            using(window.DeferDrawing())
            {
                const int smallX = 50, largeX = 150;
                const int smallY = 20, largeY = 60;
                Size
                    small = new Size(smallX, smallY),
                    large = new Size(largeX, largeY),
                    longer = new Size(smallX, largeY),
                    wider = new Size(largeX, smallY);

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

            await window.WaitForCloseAsync();
        }
    }
}
