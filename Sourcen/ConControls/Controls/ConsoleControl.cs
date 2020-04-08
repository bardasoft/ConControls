namespace ConControls.Controls
{
    /// <summary>
    /// The base class for all custom console controls.
    /// </summary>
    public class ConsoleControl : ConsoleControlBase
    {
        /// <inheritdoc />
        public ConsoleControl(IConsoleWindow window)
            : base(window) { }

    }
}
