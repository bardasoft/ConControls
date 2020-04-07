using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [StructLayout(LayoutKind.Sequential)]
    struct CHAR_INFO
    {
        public char Char;
        public ConCharAttributes Attributes;

        public CHAR_INFO(char c, ConCharAttributes attributes)
        {
            Char = c;
            Attributes = attributes;
        }
    }
}
