using System;
using System.Globalization;
using ConControls.Properties;

namespace ConControls.WindowsApi
{
    static class Exceptions
    {
        internal static InvalidOperationException CanOnlyUseSingleContext() => new InvalidOperationException(Resources.Exception_CanOnlyUseSingleContext);
        internal static ArgumentOutOfRangeException WidthTooSmall(string parameterName, int minValue, int actualValue) => new ArgumentOutOfRangeException(paramName: parameterName, actualValue: actualValue, message: string.Format(CultureInfo.CurrentCulture, Resources.Exception_WidthTooSmall, minValue));
        internal static ArgumentOutOfRangeException HeightTooSmall(string parameterName, int minValue, int actualValue) => new ArgumentOutOfRangeException(paramName: parameterName, actualValue: actualValue, message: string.Format(CultureInfo.CurrentCulture, Resources.Exception_HeightTooSmall, minValue));
    }
}
