/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Drawing;
using ConControls.WindowsApi;

namespace ConControls.Controls.Drawing 
{
    interface IProvideConsoleGraphics
    {
        IConsoleGraphics Provide(ConsoleOutputHandle consoleOutputHandle, INativeCalls api, Size size, FrameCharSets frameCharSets);
    }
}
