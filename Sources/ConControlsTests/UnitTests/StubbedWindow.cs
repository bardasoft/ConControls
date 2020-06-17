/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedWindow : StubIConsoleWindow
    {
        public ConsoleControl? Focused { get; set; }
        public StubIConsoleGraphics Graphics { get; } = new StubIConsoleGraphics();
        public ControlCollection Controls { get; }

        internal StubbedWindow()
        {
            var syncobject = new object();
            SynchronizationLockGet = () => syncobject;
            GetGraphics = () => Graphics;
            PointToClientPoint = p => p;
            PointToConsolePoint = p => p;
            WindowGet = () => this;
            Controls = new ControlCollection(this);
            ControlsGet = () => Controls;
            EnabledGet = () => true;
            VisibleGet = () => true;
            DefaultForegroundColorGet = () => ConsoleColor.Gray;
            DefaultBackgroundColorGet = () => ConsoleColor.Black;
            DefaultBorderColorGet = () => ConsoleColor.Yellow;
            DefaultCursorSizeGet = () => 1;
            FocusedControlGet = () => Focused;
            FocusedControlSetConsoleControl = c => Focused = c;
        }
    }
}
