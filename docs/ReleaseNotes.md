# ConControls - Release notes

### Version 0.2.0 (2020/06/17)

- [issue #13](https://github.com/ReneVogt/ConControls/issues/13): abstracted generic mouse events in `ConsoleControl` into six semantig virtual handler methods
- [Issue #18](https://github.com/ReneVogt/ConControls/issues/18): changed `TextControl.Wrap` from a `bool` to an enum value of `WrapMode`
- added build configuration `DebugNoSign` to build and test without the `*.pfx` key file.

### Version 0.1.1 (2020/06/16)

Migrated `CoordinateExtensions` from the unit test project into the library.  
These extensions enable consumers to easily create `Point`, `Size` or `Rectangle` instances
from tuples:

    var location = (5, 5).Pt();
    var size = (30, 20).Sz();
    var area = (location, size).Rect();

Fixed [issue #15](https://github.com/ReneVogt/ConControls/issues/15).

### Version 0.1.0 (2020/06/15)

The beta-time is over. There aren't any real beta testers anyway, and the nuget version has enough parts to
track changes (semantically).

Introduced the `Label` control.  
Refactored `TextControl` properties (not focusable and no tab stop by default,
introduced `CanEdit` property and set to `false` per default).  

### Version 0.1.0-beta-4 (2020/05/29)

Thank god it's beta! Letting the `BorderStyle` default to the parent's value is such a nonsense...  
Removed it.

### Version 0.1.0-beta-3 (2020/05/29)

Changed control behaviour to default the
- `ForegroundColor`
- `BackgroundColor`
- `BorderColor`
- `BorderStyle` and
- `CursorSize`

properties to the parent's values or finally to the window's `Default*` properties (Issue #1).  
Added `TabStop` property to decide wether a focusable control takes focus when tabbing through the window (from Issue #9).  
Fixed exception for too small areas in TextControl.

### Version 0.1.0-beta-2 (2020/05/26)

Added WaitForCloseAsync to wait for the ConsoleWindow to be closed.  
Added KeyCombination and default keys for switching console buffers and closing the ConsoleWindow.  
Fixed TextControl: redraw after Text changed.

### Version 0.1.0-beta-1 (2020/05/25)

This is the first published version of ConControls. There is still a lot missing for a 1.0, but tests can begin.

---
Ren&eacute; Vogt  
Dresden 2020/06/17