/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ConControls.WindowsApi.Types
{
    [ExcludeFromCodeCoverage]
    struct COLORREF
    {
        internal uint color;
        internal Color Color => Color.FromArgb(red: (int)(color & 0x000000FF), green: (int)((color >> 8) & 0x000000FF), blue: (int)((color >> 16) & 0x000000FF));
        internal COLORREF(Color c) => color = c.R + (uint)(c.G << 8) + (uint)(c.B << 16);
    }
}
