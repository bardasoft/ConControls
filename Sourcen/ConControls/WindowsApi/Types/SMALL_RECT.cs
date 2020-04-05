using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }
}
