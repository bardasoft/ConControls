/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls.WindowsApi;

namespace ConControls.Controls.Drawing 
{
    [ExcludeFromCodeCoverage]
    sealed class ConsoleGraphicsProvider : IProvideConsoleGraphics
    {
        public IConsoleGraphics Provide(ConsoleOutputHandle consoleOutputHandle, INativeCalls api, Size size, FrameCharSets frameCharSets) =>
            new ConsoleGraphics(consoleOutputHandle, api, size, frameCharSets);
    }
}
