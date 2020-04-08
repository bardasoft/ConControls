/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ConControls.WindowsApi.Types
{
    [ExcludeFromCodeCoverage]
    [StructLayout(LayoutKind.Sequential)]
    struct COORD
    {
        public ushort X;
        public ushort Y;

        public COORD(int x, int y)
        {
            X = (ushort)x;
            Y = (ushort)y;
        }
        public COORD(Size size)
            : this(size.Width, size.Height) { }
        public COORD(Point size)
            : this(size.X, size.Y) { }
        public COORD(Rectangle rect)
            : this(rect.Width, rect.Height) { }
    }
}
