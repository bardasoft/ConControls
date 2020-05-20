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
using ConControls.Controls.Text.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedConsoleTextController : StubIConsoleTextController
    {
        Size size;
        Point caretPosition;
        bool caretVisible;

        internal StubbedConsoleTextController()
        {
            SizeGet = () => size;
            SizeSetSize = value =>
            {
                if (size == value) return;
                size = value;
                BufferChangedEvent?.Invoke(this, EventArgs.Empty);
            };
            CaretPositionGet = () => caretPosition;
            CaretPositionSetPoint = value =>
            {
                if (value == caretPosition) return;
                caretPosition = value;
                CaretChangedEvent?.Invoke(this, EventArgs.Empty);
            };
            CaretVisibleGet = () => caretVisible;
            CaretVisibleSetBoolean = value =>
            {
                if (value == caretVisible) return;
                caretVisible = value;
                CaretChangedEvent?.Invoke(this, EventArgs.Empty);
            };
            BufferGet = () => new char[size.Width + size.Height];
        }
    }
}
