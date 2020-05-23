/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls;
using ConControls.Controls.Text;

namespace ConControlsTests.UnitTests 
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedTextControl : TextControl
    {
        /// <inheritdoc />
        internal StubbedTextControl(IConsoleWindow window)
            : this(window, null) { }
        /// <inheritdoc />
        internal StubbedTextControl(IConsoleWindow window, IConsoleTextController? textController)
            : base(window, textController ?? new StubbedConsoleTextController()) { }

        internal void CallDrawClientAreaWithNull() => DrawClientArea(null!);
    }
}
