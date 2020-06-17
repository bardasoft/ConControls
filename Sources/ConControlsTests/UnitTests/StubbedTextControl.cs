/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls;
using ConControls.Controls.Drawing;
using ConControls.Controls.Text;

namespace ConControlsTests.UnitTests 
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedTextControl : TextControl
    {
        internal StubbedTextControl() : this(null) { }
        /// <inheritdoc />
        internal StubbedTextControl(IConsoleWindow? window)
            : this(window, null) { }
        /// <inheritdoc />
        internal StubbedTextControl(IConsoleWindow? window, IConsoleTextController? textController)
            : base(window ?? new StubbedWindow(), textController ?? new StubbedConsoleTextController())
        {
        }

        internal void CallDrawClientArea(IConsoleGraphics? graphics = null) => DrawClientArea(graphics!);
        internal void CallOnMouseClick(MouseEventArgs e) => OnMouseClick(e);
        internal void CallOnMouseScroll(MouseEventArgs e) => OnMouseScroll(e);
    }
}
