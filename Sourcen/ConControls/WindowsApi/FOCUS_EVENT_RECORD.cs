/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.WindowsApi
{ 
    struct FOCUS_EVENT_RECORD
    {
        // DON'T marshal to bool, because this would break the other unioned structs!
        public int SetFocus;
    }
}
