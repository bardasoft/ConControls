namespace ConControls.Controls 
{
    /// <summary>
    /// Arguments for the <see cref="ControlCollection.ControlAdded"/> and
    /// <see cref="ControlCollection.ControlRemoved"/> events.
    /// </summary>
    public sealed class ControlCollectionChangedEventArgs
    {
        /// <summary>
        /// The <see cref="ConsoleControl"/> that has been added or removed to
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public ConsoleControl Control { get; }

        internal ControlCollectionChangedEventArgs(ConsoleControl control) => Control = control;
    }
}
