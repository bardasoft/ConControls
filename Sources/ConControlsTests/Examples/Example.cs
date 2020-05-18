/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using ConControls.Logging;

namespace ConControlsTests.Examples
{
    [ExcludeFromCodeCoverage]
    abstract class Example
    {
        public abstract DebugContext DebugContext { get; }
        public abstract Task RunAsync();
    }
}
