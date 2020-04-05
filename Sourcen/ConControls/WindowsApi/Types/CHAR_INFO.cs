using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct CHAR_INFO
    {
        public char Char;
        public ConCharAttributes Attributes;
    }
}
