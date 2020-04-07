using System.Drawing;

namespace ConControls.WindowsApi.Types
{
    struct COLORREF
    {
        internal uint color;
        internal Color Color => Color.FromArgb(red: (int)(color & 0x000000FF), green: (int)((color >> 8) & 0x000000FF), blue: (int)((color >> 16) & 0x000000FF));
        internal COLORREF(Color c) => color = c.R + (uint)(c.G << 8) + (uint)(c.B << 16);
    }
}
