using System.Drawing;
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

        public SMALL_RECT(int left, int top, int right, int bottom)
        {
            Left = (short)left;
            Top = (short)top;
            Right = (short)right;
            Bottom = (short)bottom;
        }
        public SMALL_RECT(Rectangle rect) : this(rect.Left, rect.Top, rect.Right, rect.Bottom) { }
    }
}
