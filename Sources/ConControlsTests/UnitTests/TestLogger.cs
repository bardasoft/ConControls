/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace ConControlsTests.UnitTests 
{
    [ExcludeFromCodeCoverage]
    sealed class TestLogger : TraceListener
    {
        readonly Action<string> handler;

        internal TestLogger(Action<string> action) => handler = action;

        public override void Write(string message)
        {
            handler(message);
        }
        /// <inheritdoc />
        public override void WriteLine(string message)
        {
            handler(message);
        }
    }
}
