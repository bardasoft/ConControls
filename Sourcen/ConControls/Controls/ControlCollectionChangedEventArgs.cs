using System.Diagnostics.CodeAnalysis;

namespace ConControls.Controls 
{
    /// <summary>
    /// Arguments for the <see cref="ControlCollection.ControlAdded"/> and
    /// <see cref="ControlCollection.ControlRemoved"/> events.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public sealed class ControlCollectionChangedEventArgs
    {
        /// <summary>
        /// The <see cref="ConsoleControl"/> that has been added to
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public ConsoleControl? AddedControl { get; }
        /// <summary>
        /// The <see cref="ConsoleControl"/> that has been removed from
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public ConsoleControl? RemovedControl { get; }

        internal ControlCollectionChangedEventArgs(ConsoleControl? addedControl = null, ConsoleControl? removedControl = null)
        {
            AddedControl = addedControl;
            RemovedControl = removedControl;
        }
    }
}
