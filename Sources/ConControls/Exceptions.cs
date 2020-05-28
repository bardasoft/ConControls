/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ConControls.Controls;
using ConControls.Properties;

namespace ConControls
{
    [ExcludeFromCodeCoverage]
    static class Exceptions
    {
        internal static InvalidOperationException CanOnlyUseSingleContext() => new InvalidOperationException(Resources.Exception_CanOnlyUseSingleContext);
        internal static ObjectDisposedException WindowDisposed() => new ObjectDisposedException(objectName: nameof(ConsoleWindow), Resources.Exception_WindowDisposed);
        internal static ObjectDisposedException ControlDisposed(string name) => new ObjectDisposedException(objectName: name, Resources.Exception_ControlDisposed);
        internal static Win32Exception Win32() => new Win32Exception(Marshal.GetLastWin32Error());
        internal static InvalidOperationException DifferentWindow() => new InvalidOperationException(Resources.Exception_DifferentWindow);
        internal static ArgumentOutOfRangeException InvalidConsoleColor(ConsoleColor color) => new ArgumentOutOfRangeException(paramName: nameof(color), color, string.Format(CultureInfo.CurrentCulture, Resources.Exception_InvalidConsoleColor, color));
        internal static ArgumentOutOfRangeException ProgressBarPercentageMustBeNonNegative() =>
            new ArgumentOutOfRangeException(Resources.Exception_ProgressBarPercentageMustBeNonNegative);
        internal static ArgumentOutOfRangeException ProgressBarPercentageMustNotBeGreaterThan1() => new ArgumentOutOfRangeException(
            Resources.Exception_ProgressBarPercentageMustNotBeGreaterThan1);
        internal static InvalidOperationException CannotFocusUnFocusableControl(string controlType) =>
            new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Exception_CannotFocusUnFocusableControl, controlType));
        internal static ArgumentNullException ControlsMustBeContained([CallerMemberName] string paramName = "Parent") => new ArgumentNullException(paramName: paramName, message: Resources.Exception_ControlsMustBeContained);
        internal static NotSupportedException WindowSizeNotSupported() => new NotSupportedException(Resources.Exception_WindowSizeNotSupported);
        internal static Win32Exception CouldNotCreateScreenBuffer() =>
            new Win32Exception(Resources.Exception_CouldNotCreateScreenBuffer, new Win32Exception(Marshal.GetLastWin32Error()));
        internal static Win32Exception CouldNotSetScreenBuffer() =>
            new Win32Exception(Resources.Exception_CouldNotSetScreenBuffer, new Win32Exception(Marshal.GetLastWin32Error()));
        internal static InvalidOperationException WindowHasNoParent() => new InvalidOperationException(Resources.Exception_WindowHasNoParent);
    }
}
