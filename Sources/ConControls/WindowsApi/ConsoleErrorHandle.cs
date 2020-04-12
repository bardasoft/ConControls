/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Win32.SafeHandles;

namespace ConControls.WindowsApi 
{
    /// <summary>
    /// A console standard error handle.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed class ConsoleErrorHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal ConsoleErrorHandle(IntPtr handle)
            : base(false)
        {
            SetHandle(handle);
        }
        protected override bool ReleaseHandle() => true;
    }
}
