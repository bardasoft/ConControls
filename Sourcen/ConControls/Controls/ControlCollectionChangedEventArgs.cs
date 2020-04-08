namespace ConControls.Controls 
{
    /// <summary>
    /// Arguments for the <see cref="ControlCollection.ControlAdded"/> and
    /// <see cref="ControlCollection.ControlRemoved"/> events.
    /// </summary>
    public sealed class ControlCollectionChangedEventArgs
    {
        /// <summary>
        /// The <see cref="ConsoleControlBase"/> that has been added or removed to
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public ConsoleControlBase Control { get; }

        internal ControlCollectionChangedEventArgs(ConsoleControlBase control) => Control = control;
    }
}
