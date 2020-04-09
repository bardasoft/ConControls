/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace ConControls.WindowsApi 
{
    /// <summary>
    /// A console input handle.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed class ConsoleInputHandle : SafeHandle
    {
        /// <inheritdoc />
        internal ConsoleInputHandle() : base(NativeMethods.GetStdHandle(NativeMethods.STDIN), false) { }
        /// <inheritdoc />
        protected override bool ReleaseHandle() => true;
        /// <inheritdoc />
        public override bool IsInvalid => handle.ToInt64() <= 0;
    }
}
