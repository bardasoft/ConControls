/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using ConControls.Logging;

namespace ConControlsTests 
{
    [ExcludeFromCodeCoverage]
    public sealed class FileLogger : IDisposable
    {
        readonly string file;
        public FileLogger(string file)
        {
            this.file = file;
            File.WriteAllText(this.file, $"[{Thread.CurrentThread.ManagedThreadId}]{nameof(ConControls)} test starting.{Environment.NewLine}");
            Logger.Logged += Write;
        }
        public void Dispose()
        {
            Logger.Logged -= Write;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void Write(string message) => File.AppendAllLines(file, new[] {message});
    }
}
