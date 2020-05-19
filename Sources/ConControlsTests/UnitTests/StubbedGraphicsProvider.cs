/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls.Drawing.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedGraphicsProvider : StubIProvideConsoleGraphics
    {
        public StubIConsoleGraphics Graphics { get; } = new StubIConsoleGraphics();

        internal StubbedGraphicsProvider()
        {
            ProvideConsoleOutputHandleINativeCallsSizeFrameCharSets = (handle, api, size, frameChars) => Graphics;
        }
    }
}
