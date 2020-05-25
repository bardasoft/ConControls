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
using System.Reflection;
using System.Threading.Tasks;
using ConControls.Controls;
using ConControlsExamples.Examples;
// ReSharper disable AccessToDisposedClosure

#nullable enable

namespace ConControlsExamples
{
    static class Program
    {
        static async Task Main()
        {
            TaskCompletionSource<int> windowCompletion = new TaskCompletionSource<int>();
            using var window = new ConsoleWindow();
            using(window.DeferDrawing())
            {
                var examples = from type in Assembly.GetExecutingAssembly().GetTypes()
                               where type.BaseType == typeof(Example)
                               orderby type.Name
                               select (Example)Activator.CreateInstance(type);
                var buttons = examples.Select((example, i) =>
                {
                    var button = new Button(window)
                    {
                        Area = new Rectangle(0, 3 * i, example.GetType().Name.Length + 4, 3),
                        Tag = example,
                        Text = example.GetType().Name,
                        Parent = window
                    };
                    button.Click += OnButtonClick;
                    return button;
                });
                window.Controls.AddRange(buttons.Cast<ConsoleControl>().ToArray());
                window.Controls.Add(new Button(window){Text = "Close", Area=new Rectangle(0, 3 * window.Controls.Count, 9, 3)});
            }

            await windowCompletion.Task;

            async void OnButtonClick(object sender, EventArgs e)
            {
                window.Dispose();
                var button = (Button)sender;
                if (button.Tag is Example example)
                    await example.RunAsync();
                windowCompletion.SetResult(0);
            }
        }
    }
}
