/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using ConControls;

namespace ConControlsTests 
{
    [ExcludeFromCodeCoverage]
    public sealed class Logger : TraceListener
    {
        readonly string file;
        public Logger(string file)
        {
            this.file = file;
            File.WriteAllText(this.file,$"[{Thread.CurrentThread.ManagedThreadId}]{nameof(ConsoleWindow)} test starting.{Environment.NewLine}");
            Debug.Listeners.Add(this);
        }
        public new void Dispose()
        {
            base.Dispose();
            Debug.Listeners.Remove(this);
        }
        /// <inheritdoc />
        public override void Write(string message)
        {
            File.AppendAllText(file, message);
        }
        /// <inheritdoc />
        public override void WriteLine(string message)
        {
            File.AppendAllLines(file, new []{message});
        }
    }
}
