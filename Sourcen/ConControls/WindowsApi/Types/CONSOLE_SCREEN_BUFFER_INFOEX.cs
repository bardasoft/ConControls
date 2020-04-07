using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct CONSOLE_SCREEN_BUFFER_INFOEX
    { 
        internal int Size;
        internal COORD BufferSize;
        internal COORD CursorPosition;
        internal ConCharAttributes Attributes;
        internal SMALL_RECT Window;
        internal COORD MaximumWindowSize;
        internal ConCharAttributes PopupAttributes;
        internal bool FullscreenSupported;
        internal COLORREF Black;
        internal COLORREF DarkBlue;
        internal COLORREF DarkGreen;
        internal COLORREF DarkCyan;
        internal COLORREF DarkRed;
        internal COLORREF DarkMagenta;
        internal COLORREF DarkYellow;
        internal COLORREF Gray;
        internal COLORREF DarkGray;
        internal COLORREF Blue;
        internal COLORREF Green;
        internal COLORREF Cyan;
        internal COLORREF Red;
        internal COLORREF Magenta;
        internal COLORREF Yellow;
        internal COLORREF White;
    }
}
