using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using ConControls.Properties;

namespace ConControls
{
    [ExcludeFromCodeCoverage]
    static class Exceptions
    {
        internal static InvalidOperationException CanOnlyUseSingleContext() => new InvalidOperationException(Resources.Exception_CanOnlyUseSingleContext);
        internal static ObjectDisposedException WindowDisposed() => new ObjectDisposedException(objectName: nameof(ConsoleWindow), Resources.Exception_WindowDisposed);
        internal static Win32Exception Win32() => new Win32Exception(Marshal.GetLastWin32Error());
        internal static InvalidOperationException CannotChangeRootPanelsParent() => new InvalidOperationException(Resources.Exception_CannotChangeRootPanelsParent);
        internal static InvalidOperationException DifferentWindow() => new InvalidOperationException(Resources.Exception_DifferentWindow);
        internal static ArgumentOutOfRangeException InvalidConsoleColor(ConsoleColor color) => new ArgumentOutOfRangeException(paramName: nameof(color), color, string.Format(CultureInfo.CurrentCulture, Resources.Exception_InvalidConsoleColor, color));
    }
}
