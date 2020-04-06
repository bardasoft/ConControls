using System;
using ConControls.WindowsApi;

namespace ConControls.Controls
{
    public abstract class ConsoleControl
    {
        public IConsoleContext Context { get; }
        protected ConsoleControl(IConsoleContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
