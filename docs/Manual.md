## What is ConControls?
- **[How does it work?](#howdoesitwork)**  
- **[How to get started?](#howtogetstarted)**

ConControls provides a simple UI framework for Windows console applications.

Every now and then we had to write little test programs for our software. For example, tools
that create a certain amount of connections to web services to test performance and parallel behaviour.  
These tools are typically implemented as console applications, because there was no need to create complex
UIs with frameworks like Windows Forms or WPF.  
But then again, a console output sometimes is a _too_ primitive output to comfortably evaluate the test results
or problems with the tools itself.  
So we needed something to nicely organize console output without having too much effort designing a graphical 
user interface.

#### How does it work? <a name="howdoesitwork"/>

When a [`ConsoleWindow`](controls/ConsoleWindow.md) is instantiated, a new console output screen buffer is created and set as active screen buffer. This
buffer is then used to "draw" the controls on.  
The original console screen buffer will not be touched, so all other output of the process to `stdout` will be written to that buffer as normal. The `ConsoleWindow` provides
an easy way (by setting the `ActiveScreen` property) to switch between the two buffers, so that the original output can always be displayed.

Secondly, a thread will be started that waits for `stdin` to get signaled. Everytime `stdin` gets signaled, the thread reads the console input. By doing this, all
mouse and keyboard input can be processed by the `ConsoleWindow` and the controls it contains.  
Of course, this means that things like `Console.ReadLine()` will no longer work properly and should not be used. Well, input should now be handled by the `ConsoleWindow`
and its controls.

`ConsoleWindow` provides a `WaitForCloseAsync()` method that can be used to let the thread that creates the window wait (no, `await`) for someone to close (and thereby `Dispose`)
the window. As stated above, input (mouse and keyboard) are now handled by the `ConsoleWindow` and on a different thread, so the creating thread should have nothing to do while the window is active.

#### How to get started? <a name="howtogetstarted"/>

Let's look at some code:

    using ConControls.Controls;
    /* ommitted .net using directives */

    async Task Main()
    {
        using var window = new ConsoleWindow
        {
            SwitchConsoleBuffersKey = KeyCombination.F11,
            CloseWindowKey = KeyCombination.AltF4,
            DefaultBackgroundColor = ConsoleColor.DarkBlue,
            Title = "ConControls example"
        };
        var panel = new Panel(window) 
        {
            Parent = window,        // important
            Area = window.Area,
            BorderStyle = BorderStyle.DoubleLined
        };
        _ = new TextBlock(window)
        {
            Parent = frame,         // important
            Area = new Rectangle(2, 2, 14, 3),
            BorderStyle = BorderStyle.SingleLined,
            BackgroundColor = ConsoleColor.Red,
            ForegroundColor = ConsoleColor.Yellow,
            Text = "Hello World!"
        };
  
        await window.WaitForCloseAsync();
    }

So, when you already used a UI framework before, this should be pretty straight forward.  
The **important** things to notice are:
- for several reasons (OOP and multithreading, I'll talk about that somewhere else), the `Parent` of a control cannot be
a constructor parameter, but has to be set as a property. The control is not even added to the window's control collection on instantiation.
- the `SwitchConsoleBufferKey` can be used (when set) to switch between the original console screen buffer and the buffer for the instantiated window
- the `CloseWindowKey` (when set) will close the window and set the state of the `Tàsk` returned by `WaitForCloseAsync()` to `Completed`. You can also add [`Button`](controls/Button.md)s
to the window and use their `Click` event, or subscribe to the window's `KeyEvent` to call `window.Close()` yourself instead.
- the appearance properties (`BackgroundColor`, `ForegroundColor`, `BorderColor` and `CursorSize`) are nullable values and always default to the
parent's values or finally to the window's respective `Default*` properties. Inherited controls may overwrite this behaviour or initialize their properties to a non-null value.

That's basically it, at least for the current (beta-)versions. Check the [ConControlsExample](https://github.com/ReneVogt/ConControls/tree/master/Sources/ConControlsExamples) project in the sources
for more examples and see the [list of currently avaiable controls](controls/index.md). There is an explanation of common properties, methods and behaviour of those controls below that list.

Finally you can take a look at the API (source code) documentation: [ConControls.chm](api/ConControls.chm).

---
Ren&eacute; Vogt  
Dresden 2020/05/29