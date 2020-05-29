# ConControls - Release notes

### Version 0.1.0-beta-4 (tba)

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
Dresden 2020/05/29