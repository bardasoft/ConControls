using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct CONSOLE_CURSOR_INFO
    {
        internal int Size;
        internal bool Visible;
    }
}
