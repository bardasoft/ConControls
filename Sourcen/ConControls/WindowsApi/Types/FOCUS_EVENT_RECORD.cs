/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace ConControls.WindowsApi.Types
{
    [ExcludeFromCodeCoverage]
    struct FOCUS_EVENT_RECORD
    {
        // DON'T marshal to bool, because this would break the other unioned structs!
        public int SetFocus;
    }
}
