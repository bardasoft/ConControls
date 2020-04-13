/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;

namespace ConControls.WindowsApi.Types
{
    /// <summary>
    /// Windows API virtual key codes (VK_ constants).
    /// </summary>
    [SuppressMessage("Design", "CA1028", Justification = "Win32 API value")]
    public enum VirtualKey : short
    {
        /// <summary>
        /// Left mouse button.
        /// </summary>
        LeftButton = 0x01,
        /// <summary>
        /// Right mouse button.
        /// </summary>
        RightButton = 0x02,
        /// <summary>
        /// Control-break processing.
        /// </summary>
        Cancel = 0x03,
        /// <summary>
        /// Middle mouse button (three-button mouse).
        /// </summary>
        MiddleButton = 0x04,
        /// <summary>
        /// X1 mouse button.
        /// </summary>
        ExtraButton1 = 0x05,
        /// <summary>
        /// X2 mouse button.
        /// </summary>
        ExtraButton2 = 0x06,
        /// <summary>
        /// Backspace key.
        /// </summary>
        Back = 0x08,
        /// <summary>
        /// Tab key.
        /// </summary>
        Tab = 0x09,
        /// <summary>
        /// Clear key.
        /// </summary>
        Clear = 0x0C,
        /// <summary>
        /// Enter key.
        /// </summary>
        Return = 0x0D,
        /// <summary>
        /// Shift key.
        /// </summary>
        Shift = 0x10,
        /// <summary>
        /// Ctrl key.
        /// </summary>
        Control = 0x11,
        /// <summary>
        /// Alt key.
        /// </summary>
        Menu = 0x12,
        /// <summary>
        /// Pause key.
        /// </summary>
        Pause = 0x13,
        /// <summary>
        /// Caps lock key.
        /// </summary>
        CapsLock = 0x14,
        /// <summary>
        /// IME Kana mode.
        /// </summary>
        Kana = 0x15,
        /// <summary>
        /// IME Hanguel mode (maintained for compatibility; use VK_HANGUL)
        /// </summary>
        Hangeul = 0x15,
        /// <summary>
        /// IME Hangul mode
        /// </summary>
        Hangul = 0x15,
        /// <summary>
        /// IME On.
        /// </summary>
        ImeOn = 0x16,
        /// <summary>
        /// IME Junja mode.
        /// </summary>
        Junja = 0x17,
        /// <summary>
        /// IME final mode.
        /// </summary>
        Final = 0x18,
        /// <summary>
        /// IME Hanja mode.
        /// </summary>
        Hanja = 0x19,
        /// <summary>
        /// IME Kanji mode.
        /// </summary>
        Kanji = 0x19,
        /// <summary>
        /// IME off.
        /// </summary>
        ImeOff = 0x1A,
        /// <summary>
        /// Esc key.
        /// </summary>
        Escape = 0x1B,
        /// <summary>
        /// IME convert.
        /// </summary>
        Convert = 0x1C,
        /// <summary>
        /// IME nonconvert.
        /// </summary>
        NonConvert = 0x1D,
        /// <summary>
        /// IME accept.
        /// </summary>
        Accept = 0x1E,
        /// <summary>
        /// IME mode change request.
        /// </summary>
        ModeChange = 0x1F,
        /// <summary>
        /// Space bar.
        /// </summary>
        Space = 0x20,
        /// <summary>
        /// Page Up key.
        /// </summary>
        Prior = 0x21,
        /// <summary>
        /// Page down key.
        /// </summary>
        Next = 0x22,
        /// <summary>
        /// End key.
        /// </summary>
        End = 0x23,
        /// <summary>
        /// Home key.
        /// </summary>
        Home = 0x24,
        /// <summary>
        /// Left arrow key.
        /// </summary>
        Left = 0x25,
        /// <summary>
        /// Up arrow key.
        /// </summary>
        Up = 0x26,
        /// <summary>
        ///  Right arrow key.
        /// </summary>
        Right = 0x27,
        /// <summary>
        /// Down arrow key.
        /// </summary>
        Down = 0x28,
        /// <summary>
        /// Select key.
        /// </summary>
        Select = 0x29,
        /// <summary>
        /// Print key.
        /// </summary>
        Print = 0x2A,
        /// <summary>
        /// Execute key.
        /// </summary>
        Execute = 0x2B,
        /// <summary>
        /// Print screen key.
        /// </summary>
        Snapshot = 0x2C,
        /// <summary>
        /// Ins key.
        /// </summary>
        Insert = 0x2D,
        /// <summary>
        /// Del key.
        /// </summary>
        Delete = 0x2E,
        /// <summary>
        /// Help key.
        /// </summary>
        Help = 0x2F,
        /// <summary>
        /// 0 key.
        /// </summary>
        N0 = 0x30,
        /// <summary>
        /// 1 key.
        /// </summary>
        N1 = 0x31,
        /// <summary>
        /// 2 key.
        /// </summary>
        N2 = 0x32,
        /// <summary>
        /// 3 key.
        /// </summary>
        N3 = 0x33,
        /// <summary>
        /// 4 key.
        /// </summary>
        N4 = 0x34,
        /// <summary>
        /// 5 key.
        /// </summary>
        N5 = 0x35,
        /// <summary>
        /// 6 key.
        /// </summary>
        N6 = 0x36,
        /// <summary>
        /// 7 key.
        /// </summary>
        N7 = 0x37,
        /// <summary>
        /// 8 key.
        /// </summary>
        N8 = 0x38,
        /// <summary>
        /// 9 key.
        /// </summary>
        N9 = 0x39,
        /// <summary>
        /// A key.
        /// </summary>
        A = 0x41,
        /// <summary>
        /// B key.
        /// </summary>
        B = 0x42,
        /// <summary>
        /// C key.
        /// </summary>
        C = 0x43,
        /// <summary>
        /// D key.
        /// </summary>
        D = 0x44,
        /// <summary>
        /// E key.
        /// </summary>
        E = 0x45,
        /// <summary>
        /// F key.
        /// </summary>
        F = 0x46,
        /// <summary>
        /// G key.
        /// </summary>
        G = 0x47,
        /// <summary>
        /// H key.
        /// </summary>
        H = 0x48,
        /// <summary>
        /// I key.
        /// </summary>
        I = 0x49,
        /// <summary>
        /// J key.
        /// </summary>
        J = 0x4A,
        /// <summary>
        /// K key.
        /// </summary>
        K = 0x4B,
        /// <summary>
        /// L key.
        /// </summary>
        L = 0x4C,
        /// <summary>
        /// M key.
        /// </summary>
        M = 0x4D,
        /// <summary>
        /// N key.
        /// </summary>
        N = 0x4E,
        /// <summary>
        /// O key.
        /// </summary>
        O = 0x4F,
        /// <summary>
        /// P key.
        /// </summary>
        P = 0x50,
        /// <summary>
        /// Q key.
        /// </summary>
        Q = 0x51,
        /// <summary>
        /// R key.
        /// </summary>
        R = 0x52,
        /// <summary>
        /// S key.
        /// </summary>
        S = 0x53,
        /// <summary>
        /// T key.
        /// </summary>
        T = 0x54,
        /// <summary>
        /// U key.
        /// </summary>
        U = 0x55,
        /// <summary>
        /// V key.
        /// </summary>
        V = 0x56,
        /// <summary>
        /// W key.
        /// </summary>
        W = 0x57,
        /// <summary>
        /// X key.
        /// </summary>
        X = 0x58,
        /// <summary>
        /// Y key.
        /// </summary>
        Y = 0x59,
        /// <summary>
        /// Z key.
        /// </summary>
        Z = 0x5A,
        /// <summary>
        /// Left Windows key (natural keyboard).
        /// </summary>
        LeftWindows = 0x5B,
        /// <summary>
        /// Right Windows key (natural keyboard).
        /// </summary>
        RightWindows = 0x5C,
        /// <summary>
        /// Application key (natural keyboard).
        /// </summary>
        Application = 0x5D,
        /// <summary>
        /// Computer sleep key.
        /// </summary>
        Sleep = 0x5F,
        /// <summary>
        /// Numeric keypad 0 key.
        /// </summary>
        Numpad0 = 0x60,
        /// <summary>
        /// Numeric keypad 1 key.
        /// </summary>
        Numpad1 = 0x61,
        /// <summary>
        /// Numeric keypad 2 key.
        /// </summary>
        Numpad2 = 0x62,
        /// <summary>
        /// Numeric keypad 3 key.
        /// </summary>
        Numpad3 = 0x63,
        /// <summary>
        /// Numeric keypad 4 key.
        /// </summary>
        Numpad4 = 0x64,
        /// <summary>
        /// Numeric keypad 5 key.
        /// </summary>
        Numpad5 = 0x65,
        /// <summary>
        /// Numeric keypad 6 key.
        /// </summary>
        Numpad6 = 0x66,
        /// <summary>
        /// Numeric keypad 7 key.
        /// </summary>
        Numpad7 = 0x67,
        /// <summary>
        /// Numeric keypad 8 key.
        /// </summary>
        Numpad8 = 0x68,
        /// <summary>
        /// Numeric keypad 9 key.
        /// </summary>
        Numpad9 = 0x69,
        /// <summary>
        /// Multiply key.
        /// </summary>
        Multiply = 0x6A,
        /// <summary>
        /// Add key.
        /// </summary>
        Add = 0x6B,
        /// <summary>
        /// Separator key.
        /// </summary>
        Separator = 0x6C,
        /// <summary>
        /// Subtract key.
        /// </summary>
        Subtract = 0x6D,
        /// <summary>
        /// Decimal key.
        /// </summary>
#pragma warning disable CA1720
        Decimal = 0x6E,
#pragma warning restore CA1720
        /// <summary>
        /// Divide key.
        /// </summary>
        Divide = 0x6F,
        /// <summary>
        /// F1 key.
        /// </summary>
        F1 = 0x70,
        /// <summary>
        /// F2 key.
        /// </summary>
        F2 = 0x71,
        /// <summary>
        /// F3 key.
        /// </summary>
        F3 = 0x72,
        /// <summary>
        /// F4 key.
        /// </summary>
        F4 = 0x73,
        /// <summary>
        /// F5 key.
        /// </summary>
        F5 = 0x74,
        /// <summary>
        /// F6 key.
        /// </summary>
        F6 = 0x75,
        /// <summary>
        /// F7 key.
        /// </summary>
        F7 = 0x76,
        /// <summary>
        /// F8 key.
        /// </summary>
        F8 = 0x77,
        /// <summary>
        /// F9 key.
        /// </summary>
        F9 = 0x78,
        /// <summary>
        /// F10 key.
        /// </summary>
        F10 = 0x79,
        /// <summary>
        /// F11 key.
        /// </summary>
        F11 = 0x7A,
        /// <summary>
        /// F12 key.
        /// </summary>
        F12 = 0x7B,
        /// <summary>
        /// F13 key.
        /// </summary>
        F13 = 0x7C,
        /// <summary>
        /// F14 key.
        /// </summary>
        F14 = 0x7D,
        /// <summary>
        /// F15 key.
        /// </summary>
        F15 = 0x7E,
        /// <summary>
        /// F16 key.
        /// </summary>
        F16 = 0x7F,
        /// <summary>
        /// F17 key.
        /// </summary>
        F17 = 0x80,
        /// <summary>
        /// F18 key.
        /// </summary>
        F18 = 0x81,
        /// <summary>
        /// F19 key.
        /// </summary>
        F19 = 0x82,
        /// <summary>
        /// F20 key.
        /// </summary>
        F20 = 0x83,
        /// <summary>
        /// F21 key.
        /// </summary>
        F21 = 0x84,
        /// <summary>
        /// F22 key.
        /// </summary>
        F22 = 0x85,
        /// <summary>
        /// F23 key.
        /// </summary>
        F23 = 0x86,
        /// <summary>
        /// F24 key.
        /// </summary>
        F24 = 0x87,
        /// <summary>
        /// NumLock key.
        /// </summary>
        NumLock = 0x90,
        /// <summary>
        /// Scroll lock key.
        /// </summary>
        ScrollLock = 0x91,
        /// <summary>
        /// Left shift key.
        /// </summary>
        LeftShift = 0xA0,
        /// <summary>
        /// Right shift key.
        /// </summary>
        RightShift = 0xA1,
        /// <summary>
        /// Left control key.
        /// </summary>
        LeftControl = 0xA2,
        /// <summary>
        /// Right control key.
        /// </summary>
        RightControl = 0xA3,
        /// <summary>
        /// Left menu key.
        /// </summary>
        LeftMenu = 0xA4,
        /// <summary>
        /// Right menu key.
        /// </summary>
        RightMenu = 0xA5,
        /// <summary>
        /// Browser Back key.
        /// </summary>
        BrowserBack = 0xA6,
        /// <summary>
        /// Browser Forward key.
        /// </summary>
        BrowserForward = 0xA7,
        /// <summary>
        /// Browser Refresh key.
        /// </summary>
        BrowserRefresh = 0xA8,
        /// <summary>
        /// Browser Stop key.
        /// </summary>
        BrowserStop = 0xA9,
        /// <summary>
        /// Browser search key.
        /// </summary>
        BrowserSearch = 0xAA,
        /// <summary>
        /// Browser Favorites key.
        /// </summary>
        BrowserFavorites = 0xAB,
        /// <summary>
        /// Browser Start and Home key.
        /// </summary>
        BrowserHome = 0xAC,
        /// <summary>
        /// Volumne Mute key.
        /// </summary>
        VolumeMute = 0xAD,
        /// <summary>
        /// Volume Down key.
        /// </summary>
        VolumeDown = 0xAE,
        /// <summary>
        /// Volumne Up key.
        /// </summary>
        VolumeUp = 0xAF,
        /// <summary>
        /// Next track key.
        /// </summary>
        MediaNextTrack = 0xB0,
        /// <summary>
        /// Previous track key.
        /// </summary>
        MediaPrevTrack = 0xB1,
        /// <summary>
        /// Stop media key.
        /// </summary>
        MediaStop = 0xB2,
        /// <summary>
        /// Play/pause media key.
        /// </summary>
        MediaPlayPause = 0xB3,
        /// <summary>
        /// Start mail key.
        /// </summary>
        LaunchMail = 0xB4,
        /// <summary>
        /// Select media key.
        /// </summary>
        LaunchMediaSelect = 0xB5,
        /// <summary>
        /// Start application 1 key.
        /// </summary>
        LaunchApplication1 = 0xB6,
        /// <summary>
        /// Start application 2 key.
        /// </summary>
        LaunchApplication2 = 0xB7,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the ';:' key.
        /// </summary>
        OEM1 = 0xBA,
        /// <summary>
        /// For any country/region, the '+' key.
        /// </summary>
        OEMPlus = 0xBB,
        /// <summary>
        /// For any country/region, the ',' key.
        /// </summary>
        OEMComma = 0xBC,
        /// <summary>
        /// For any country/region, the '-' key.
        /// </summary>
        OEMMinus = 0xBD,
        /// <summary>
        /// For any country/region, the '.' key.
        /// </summary>
        OEMPeriod = 0xBE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the '/?' key.
        /// </summary>
        OEM2 = 0xBF,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the '`~' key.
        /// </summary>
        OEM3 = 0xC0,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the '[{' key.
        /// </summary>
        OEM4 = 0xDB,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the '\|' key.
        /// </summary>
        OEM5 = 0xDC,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the ']}' key.
        /// </summary>
        OEM6 = 0xDD,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// For the US standard keyboard, the 'single-quote/double-quote' key.
        /// </summary>
        OEM7 = 0xDE,
        /// <summary>
        /// Used for miscellaneous characters; it can vary by keyboard.
        /// </summary>
        OEM8 = 0xDF,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMAX = 0xE1,
        /// <summary>
        /// Either the angle bracket key or the backslash key on the RT 102-key keyboard.
        /// </summary>
        OEM102 = 0xE2,
        /// <summary>
        /// OEM specific.
        /// </summary>
        ICOHelp = 0xE3,
        /// <summary>
        /// OEM specific.
        /// </summary>
        ICO00 = 0xE4,
        /// <summary>
        /// IME process key.
        /// </summary>
        ProcessKey = 0xE5,
        /// <summary>
        /// OEM specific.
        /// </summary>
        ICOClear = 0xE6,
        /// <summary>
        /// Used to pass Unicode characters as if they were keystrokes.
        /// </summary>
        Packet = 0xE7,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMReset = 0xE9,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMJump = 0xEA,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMPA1 = 0xEB,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMPA2 = 0xEC,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMPA3 = 0xED,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMWSCtrl = 0xEE,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMCUSel = 0xEF,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMATTN = 0xF0,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMFinish = 0xF1,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMCopy = 0xF2,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMAuto = 0xF3,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMENLW = 0xF4,
        /// <summary>
        /// OEM specific.
        /// </summary>
        OEMBackTab = 0xF5,
        /// <summary>
        /// Attn key.
        /// </summary>
        ATTN = 0xF6,
        /// <summary>
        /// CrSel key.
        /// </summary>
        CRSel = 0xF7,
        /// <summary>
        /// ExSel key.
        /// </summary>
        EXSel = 0xF8,
        /// <summary>
        /// Erase EOF key.
        /// </summary>
        EREOF = 0xF9,
        /// <summary>
        /// Play key.
        /// </summary>
        Play = 0xFA,
        /// <summary>
        /// Zoom key.
        /// </summary>
        Zoom = 0xFB,
        /// <summary>
        /// PA1 key.
        /// </summary>
        PA1 = 0xFD,
        /// <summary>
        /// Clear key.
        /// </summary>
        OEMClear = 0xFE
    }
}
