using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ConControls.Controls
{
    /// <summary>
    /// A collection of (child) <see cref="ConsoleControlBase"/>s.
    /// </summary>
    public sealed class ControlCollection : IEnumerable<ConsoleControlBase>
    {
        readonly ConsoleControlBase owner;
        readonly List<ConsoleControlBase> controls = new List<ConsoleControlBase>();

        /// <summary>
        /// Raised when a <see cref="ConsoleControlBase"/> is added to this
        /// <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler<ControlCollectionChangedEventArgs>? ControlAdded;
        /// <summary>
        /// Raised when a <see cref="ConsoleControlBase"/> is removed from this
        /// <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler<ControlCollectionChangedEventArgs>? ControlRemoved;

        /// <summary>
        /// Gets the <see cref="ConsoleControlBase"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="ConsoleControlBase"/> in this collection.</param>
        /// <returns>The <see cref="ConsoleControlBase"/> at the given <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The <paramref name="index"/> was outside this collection.</exception>
        public ConsoleControlBase this[int index] => controls[index];

        internal ControlCollection(ConsoleControlBase owner) => this.owner = owner;

        /// <summary>
        /// Adds the given <paramref name="control"/> to the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControlBase"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(ConsoleControlBase control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (controls.Contains(control)) return;
            if (control.Window != owner.Window) throw Exceptions.DifferentWindow();
            controls.Add(control);
            ControlAdded?.Invoke(this, new ControlCollectionChangedEventArgs(control));
        }
        /// <summary>
        /// Removes the given <paramref name="control"/> from the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControlBase"/> to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(ConsoleControlBase control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (!controls.Contains(control)) return;
            controls.Remove(control);
            ControlRemoved?.Invoke(this, new ControlCollectionChangedEventArgs(control));
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerator<ConsoleControlBase> GetEnumerator() => ((IEnumerable<ConsoleControlBase>)controls.ToArray()).GetEnumerator();
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
