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
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConControlsTests 
{
    [ExcludeFromCodeCoverage]
    public sealed class FileLogger : TraceListener
    {
        readonly string file;
        public FileLogger(string file)
        {
            this.file = file;
            File.WriteAllText(this.file, $"[{Thread.CurrentThread.ManagedThreadId}]{nameof(ConControls)} test starting.{Environment.NewLine}");
            Debug.Listeners.Add(this);
        }
        protected override void Dispose(bool disposing)
        {
            Debug.Listeners.Remove(this);
            base.Dispose(disposing);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(string message) => File.AppendAllText(file, message);
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string message) => File.AppendAllLines(file, new[] {message});
    }
}
