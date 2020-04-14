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
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConControls.Logging
{
    [ExcludeFromCodeCoverage]
    static class Logger
    {
        internal static DebugContext Context { get; set; }
        internal static event Action<string>? Logged;
        
        [Conditional("DEBUG")]
        internal static void Log(DebugContext context, string msg, [CallerFilePath] string callerFile = "?", [CallerMemberName] string callerMember = "?")
        {
            if (((int)Context & (int)context) == 0) return;
            Logged?.Invoke($"[{Thread.CurrentThread.ManagedThreadId}]{Path.GetFileNameWithoutExtension(callerFile)}.{callerMember}: {msg}");
        }
    }
}
