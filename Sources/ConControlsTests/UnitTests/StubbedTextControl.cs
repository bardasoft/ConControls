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
        internal StubbedTextControl(IControlContainer parent)
            : this(parent, null) { }
        /// <inheritdoc />
        internal StubbedTextControl(IControlContainer parent, IConsoleTextController? textController)
            : base(parent, textController ?? new StubbedConsoleTextController()) { }
    }
}
