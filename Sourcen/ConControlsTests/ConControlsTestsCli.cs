/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.ComponentModel;
using System.Runtime.InteropServices;

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
// ReSharper disable UnusedMember.Local

namespace ConControlsTests
{
    static class ConControlsTestsCli
    {
        static void Main() { }
        static string GetLastError() => new Win32Exception(Marshal.GetLastWin32Error()).Message;
    }
}
