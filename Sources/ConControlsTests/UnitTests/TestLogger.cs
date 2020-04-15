/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#nullable enable

namespace ConControlsTests.UnitTests
{
    [ExcludeFromCodeCoverage]
    class TestLogger : TraceListener
    {
        readonly Action<string> handler;
        internal TestLogger(Action<string> handler)
        {
            this.handler = handler;
            Debug.Listeners.Add(this);
        }
        protected override void Dispose(bool disposing)
        {
            Debug.Listeners.Remove(this);
            base.Dispose(disposing);
        }
        public override void Write(string message) => handler(message);
        public override void WriteLine(string message) => handler(message);
    }
}
