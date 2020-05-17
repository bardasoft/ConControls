/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedWindow : StubIConsoleWindow
    {
        public StubIConsoleGraphics Graphics { get; set; } = new StubIConsoleGraphics();

        internal StubbedWindow()
        {
            var syncobject = new object();
            SynchronizationLockGet = () => syncobject;
            GetGraphics = () => Graphics;
            PointToClientPoint = p => p;
            PointToConsolePoint = p => p;
            WindowGet = () => this;
            var controls = new ConControls.Controls.ControlCollection(this);
            ControlsGet = () => controls;
        }
    }
}
