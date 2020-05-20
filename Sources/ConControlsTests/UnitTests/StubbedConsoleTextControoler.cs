/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls.Text.Fakes;

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedConsoleTextController : StubIConsoleTextController
    {
    }
}
