/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace ConControls.Controls
{
    /// <summary>
    /// A collection of (child) <see cref="ConsoleControl"/>s.
    /// </summary>
    public sealed class ControlCollection : IEnumerable<ConsoleControl>
    {
        readonly IControlContainer container;
        readonly object syncLock;
        readonly List<ConsoleControl> controls = new List<ConsoleControl>();

        /// <summary>
        /// Raised when one or more <see cref="ConsoleControl"/> instances
        /// have been added to or removed from this <see cref="ControlCollection"/>.
        /// </summary>
        public event EventHandler<ControlCollectionChangedEventArgs>? ControlCollectionChanged;

        /// <summary>
        /// Gets the <see cref="ConsoleControl"/> at the given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the <see cref="ConsoleControl"/> in this collection.</param>
        /// <returns>The <see cref="ConsoleControl"/> at the given <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">The <paramref name="index"/> was outside this collection.</exception>
        public ConsoleControl this[int index]
        {
            get
            {
                lock (syncLock) return controls[index];
            }
        }
        /// <summary>
        /// Gets the number of controls in this collection.
        /// </summary>
        public int Count
        {
            get
            {
                lock (syncLock) return controls.Count;
            }
        }

        internal ControlCollection(IControlContainer container) => (this.container, syncLock) = (container, container.Window.SynchronizationLock);

        /// <summary>
        /// Adds the given <paramref name="control"/> to the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControl"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="control"/> uses a different <see cref="IConsoleWindow"/> than this collection.</exception>
        public void Add(ConsoleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            lock (syncLock)
            {
                if (control.Window != container.Window) throw Exceptions.DifferentWindow();
                if (controls.Contains(control)) return;
                controls.Add(control);
                control.Parent = container;
            }

            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Added(control));
        }
        /// <summary>
        /// Adds a sequence of <see cref="ConsoleControl"/> instances to the collection.
        /// </summary>
        /// <param name="controlsToAdd">The sequence of <see cref="ConsoleControl"/> instances to add.</param>
        /// <exception cref="InvalidOperationException">One or more controls in <paramref name="controlsToAdd"/> use a different <see cref="IConsoleWindow"/> than this collection.</exception>
        public void AddRange(params ConsoleControl[] controlsToAdd) => AddRange((IEnumerable<ConsoleControl>)controlsToAdd);
        /// <summary>
        /// Adds a sequence of <see cref="ConsoleControl"/> instances to the collection.
        /// </summary>
        /// <param name="controlsToAdd">The sequence of <see cref="ConsoleControl"/> instances to add.</param>
        /// <exception cref="InvalidOperationException">One or more controls in <paramref name="controlsToAdd"/> use a different <see cref="IConsoleWindow"/> than this collection.</exception>
        public void AddRange(IEnumerable<ConsoleControl> controlsToAdd)
        {
            ControlCollectionChangedEventArgs e;

            lock (syncLock)
            {
                var range = controlsToAdd.Distinct()
                                         .Where(control => control != null)
                                         .Except(controls)
                                         .ToList();
                if (range.Any(control => control.Window != container.Window)) throw Exceptions.DifferentWindow();
                if (range.Count == 0) return;
                controls.AddRange(range);
                range.ForEach(c => c.Parent = container);
                e = ControlCollectionChangedEventArgs.Added(range);
            }

            ControlCollectionChanged?.Invoke(this, e);
        }
        /// <summary>
        /// Removes the given <paramref name="control"/> from the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControl"/> to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <c>null</c>.</exception>
        public void Remove(ConsoleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            lock (syncLock)
            {
                if (!controls.Contains(control)) return;
                controls.Remove(control);
                if (control.Parent == container) control.Parent = null;
            }

            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Removed(control));
        }
        /// <summary>
        /// Removes the given sequence of <see cref="ConsoleControl"/> instances
        /// from the collection.
        /// </summary>
        /// <param name="controlsToRemove">The sequence of <see cref="ConsoleControl"/> instances
        /// to remove.</param>
        public void RemoveRange(params ConsoleControl[] controlsToRemove) => RemoveRange((IEnumerable<ConsoleControl>)controlsToRemove);
        /// <summary>
        /// Removes the given sequence of <see cref="ConsoleControl"/> instances
        /// from the collection.
        /// </summary>
        /// <param name="controlsToRemove">The sequence of <see cref="ConsoleControl"/> instances
        /// to remove.</param>
        public void RemoveRange(IEnumerable<ConsoleControl> controlsToRemove)
        {
            ControlCollectionChangedEventArgs e;
            lock (syncLock)
            {
                var range = new HashSet<ConsoleControl>(controlsToRemove.Intersect(controls)).ToList();
                if (range.Count == 0) return;
                controls.RemoveAll(control => range.Contains(control));
                range.ForEach(c =>
                {
                    if (c.Parent == container) c.Parent = null;
                });
                e = ControlCollectionChangedEventArgs.Removed(range);
            }

            ControlCollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Determines the index (position) of the <paramref name="control"/> in this collection.
        /// </summary>
        /// <param name="control">The control to look for.</param>
        /// <returns>The index of the <paramref name="control"/> in the collection, or <c>-1</c> if this collection does not contain the control.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <c>null</c>.</exception>
        public int IndexOf(ConsoleControl control)
        {
            _ = control ?? throw new ArgumentNullException(paramName: nameof(control));
            lock(syncLock) return controls.IndexOf(control);
        }

        /// <summary>
        /// Gets an <see cref="IEnumerator{ConsoleControl}"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{ConsoleControl}"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.</returns>
        public IEnumerator<ConsoleControl> GetEnumerator()
        {
            lock (syncLock)
                return ((IEnumerable<ConsoleControl>)controls.ToArray()).GetEnumerator();
        }
        /// <summary>
        /// Gets an <see cref="IEnumerator"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.</returns>
        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
