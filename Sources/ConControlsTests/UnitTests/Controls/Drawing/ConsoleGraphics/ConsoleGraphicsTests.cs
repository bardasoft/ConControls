/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.Drawing.ConsoleGraphics
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public partial class ConsoleGraphicsTests
    {
        const char cc1 = 'A';
        const char cc2 = 'B';
        const char cc3 = 'C';
        const char cc4 = 'D';
        const ConsoleColor foreground = ConsoleColor.Blue;
        const ConsoleColor background = ConsoleColor.Green;
        static readonly ConCharAttributes attributes = foreground.ToForegroundColor() | background.ToBackgroundColor();
        static readonly CHAR_INFO cA = new CHAR_INFO
        {
            Attributes = attributes,
            Char = cc1
        };
        static readonly CHAR_INFO cB = new CHAR_INFO
        {
            Attributes = attributes,
            Char = cc2
        };
        static readonly CHAR_INFO cC = new CHAR_INFO
        {
            Attributes = attributes,
            Char = cc3
        };
        static readonly CHAR_INFO cD = new CHAR_INFO
        {
            Attributes = attributes,
            Char = cc4
        };
        static readonly CHAR_INFO cc = new CHAR_INFO
        {
            Attributes = attributes,
            Char = (char)0xFFFF
        };

    }
}
