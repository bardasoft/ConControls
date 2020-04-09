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
    /// A console output handle.
    /// </summary>
    [ExcludeFromCodeCoverage]
    sealed class ConsoleOutputHandle : SafeHandle
    {
        /// <inheritdoc />
        internal ConsoleOutputHandle() : base(NativeMethods.GetStdHandle(NativeMethods.STDOUT), false) { }
        /// <inheritdoc />
        protected override bool ReleaseHandle() => true;
        /// <inheritdoc />
        public override bool IsInvalid => handle.ToInt64() <= 0;
    }
}
