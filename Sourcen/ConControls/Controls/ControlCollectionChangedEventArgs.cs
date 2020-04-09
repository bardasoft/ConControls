/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
        /// The collection of <see cref="ConsoleControl"/> instances that have been added to
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public IReadOnlyCollection<ConsoleControl> AddedControls { get; }
        /// <summary>
        /// The collection of <see cref="ConsoleControl"/> instances that have been removed from
        /// the <see cref="ControlCollection"/>.
        /// </summary>
        public IReadOnlyCollection<ConsoleControl> RemovedControls { get; }

        ControlCollectionChangedEventArgs(IEnumerable<ConsoleControl> added, IEnumerable<ConsoleControl> removed)
        {
            AddedControls = added.ToList().AsReadOnly();
            RemovedControls = removed.ToList().AsReadOnly();
        }

        internal static ControlCollectionChangedEventArgs Added(ConsoleControl control) =>
            new ControlCollectionChangedEventArgs(
                new[] { control ?? throw new ArgumentNullException(nameof(control)) },
                Enumerable.Empty<ConsoleControl>());
        internal static ControlCollectionChangedEventArgs Added(IEnumerable<ConsoleControl> controls) =>
            new ControlCollectionChangedEventArgs(
                controls ?? throw new ArgumentOutOfRangeException(nameof(controls)),
                Enumerable.Empty<ConsoleControl>());
        internal static ControlCollectionChangedEventArgs Removed(ConsoleControl control) =>
            new ControlCollectionChangedEventArgs(
                Enumerable.Empty<ConsoleControl>(),
                new[] {control ?? throw new ArgumentNullException(nameof(control))});
        internal static ControlCollectionChangedEventArgs Removed(IEnumerable<ConsoleControl> controls) =>
            new ControlCollectionChangedEventArgs(
                Enumerable.Empty<ConsoleControl>(),
                controls ?? throw new ArgumentOutOfRangeException(nameof(controls)));
    }
}
