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
        [Conditional("DEBUG")]
        internal static void Log(DebugContext context, string msg, [CallerFilePath] string callerFile = "?", [CallerMemberName] string callerMember = "?")
        {
            if (((int)Context & (int)context) == 0) return;
            Debug.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]{Path.GetFileNameWithoutExtension(callerFile)}.{callerMember}: {msg}");
        }

    }
}
