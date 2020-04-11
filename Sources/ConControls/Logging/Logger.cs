using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ConControls.Logging
{
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
