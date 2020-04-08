using System;
using ConControls.WindowsApi.Types;

namespace ConControls.ConsoleApi
{
    static class CharInfoExtensions
    {
        internal static ConCharAttributes ToForegroundColor(this ConsoleColor color) => color switch
        {
            ConsoleColor.Black => ConCharAttributes.None,
            ConsoleColor.DarkBlue => ConCharAttributes.ForeGroundBlue,
            ConsoleColor.DarkGreen => ConCharAttributes.ForeGroundGreen,
            ConsoleColor.DarkCyan => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen,
            ConsoleColor.DarkRed => ConCharAttributes.ForeGroundRed,
            ConsoleColor.DarkMagenta => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundRed,
            ConsoleColor.DarkYellow => ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed,
            ConsoleColor.Gray => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed,
            ConsoleColor.DarkGray => ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Blue => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Green => ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Cyan => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Red => ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Magenta => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.Yellow => ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity,
            ConsoleColor.White => ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed |
                                  ConCharAttributes.ForeGroundIntensity,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
        internal static ConCharAttributes ToBackgroundColor(this ConsoleColor color) => color switch
        {
            ConsoleColor.Black => ConCharAttributes.None,
            ConsoleColor.DarkBlue => ConCharAttributes.BackGroundBlue,
            ConsoleColor.DarkGreen => ConCharAttributes.BackGroundGreen,
            ConsoleColor.DarkCyan => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen,
            ConsoleColor.DarkRed => ConCharAttributes.BackGroundRed,
            ConsoleColor.DarkMagenta => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundRed,
            ConsoleColor.DarkYellow => ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed,
            ConsoleColor.Gray => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed,
            ConsoleColor.DarkGray => ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Blue => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Green => ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Cyan => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Red => ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Magenta => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.Yellow => ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity,
            ConsoleColor.White => ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed |
                                  ConCharAttributes.BackGroundIntensity,
            _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
        };
        internal static CHAR_INFO SetBackground(this CHAR_INFO info, ConsoleColor color) =>
            new CHAR_INFO(info.Char, (ConCharAttributes)((int)info.Attributes & 0xFF0F) | color.ToBackgroundColor());
        internal static CHAR_INFO SetForeground(this CHAR_INFO info, ConsoleColor color) =>
            new CHAR_INFO(info.Char, (ConCharAttributes)((int)info.Attributes & 0xFFF0) | color.ToForegroundColor());
        internal static CHAR_INFO SetChar(this CHAR_INFO info, char c) => new CHAR_INFO(c, info.Attributes);
        internal static CHAR_INFO AsCharInfo(this char c) => new CHAR_INFO(c, ConCharAttributes.None);
    }
}
