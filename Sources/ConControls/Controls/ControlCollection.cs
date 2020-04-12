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
using System.Runtime.CompilerServices;

namespace ConControls.Controls
{
    /// <summary>
    /// A collection of (child) <see cref="ConsoleControl"/>s.
    /// </summary>
    public sealed class ControlCollection : IEnumerable<ConsoleControl>
    {
        readonly IConsoleWindow window;
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
        public ConsoleControl this[int index] => controls[index];
        /// <summary>
        /// Gets the number of controls in this collection.
        /// </summary>
        public int Count => controls.Count;

        internal ControlCollection(IConsoleWindow window) => this.window = window;

        /// <summary>
        /// Adds the given <paramref name="control"/> to the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControl"/> to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        /// <exception cref="InvalidOperationException">The <paramref name="control"/> uses a different <see cref="IConsoleWindow"/> than this collection.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Add(ConsoleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (controls.Contains(control)) return;
            if (control.Window != window) throw Exceptions.DifferentWindow();
            controls.Add(control);
            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Added(control));
        }
        /// <summary>
        /// Adds a sequence of <see cref="ConsoleControl"/> instances to the collection.
        /// </summary>
        /// <param name="controlsToAdd">The sequence of <see cref="ConsoleControl"/> instances to add.</param>
        /// <exception cref="InvalidOperationException">One or more controls in <paramref name="controlsToAdd"/> use a different <see cref="IConsoleWindow"/> than this collection.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddRange(params ConsoleControl[] controlsToAdd) => AddRange((IEnumerable<ConsoleControl>)controlsToAdd);
        /// <summary>
        /// Adds a sequence of <see cref="ConsoleControl"/> instances to the collection.
        /// </summary>
        /// <param name="controlsToAdd">The sequence of <see cref="ConsoleControl"/> instances to add.</param>
        /// <exception cref="InvalidOperationException">One or more controls in <paramref name="controlsToAdd"/> use a different <see cref="IConsoleWindow"/> than this collection.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddRange(IEnumerable<ConsoleControl> controlsToAdd)
        {
            var range = controlsToAdd.Distinct()
                                     .Where(control => control != null)
                                     .Except(controls).ToArray();
            if (range.Any(control => control.Window != window)) throw Exceptions.DifferentWindow();
            if (range.Length == 0) return;
            controls.AddRange(range);
            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Added(range));
        }
        /// <summary>
        /// Removes the given <paramref name="control"/> from the collection.
        /// </summary>
        /// <param name="control">The <see cref="ConsoleControl"/> to remove.</param>
        /// <exception cref="ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(ConsoleControl control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            if (!controls.Contains(control)) return;
            controls.Remove(control);
            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Removed(control));
        }
        /// <summary>
        /// Removes the given sequence of <see cref="ConsoleControl"/> instances
        /// from the collection.
        /// </summary>
        /// <param name="controlsToRemove">The sequence of <see cref="ConsoleControl"/> instances
        /// to remove.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveRange(params ConsoleControl[] controlsToRemove) => RemoveRange((IEnumerable<ConsoleControl>)controlsToRemove);
        /// <summary>
        /// Removes the given sequence of <see cref="ConsoleControl"/> instances
        /// from the collection.
        /// </summary>
        /// <param name="controlsToRemove">The sequence of <see cref="ConsoleControl"/> instances
        /// to remove.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveRange(IEnumerable<ConsoleControl> controlsToRemove)
        {
            var range = new HashSet<ConsoleControl>(controlsToRemove.Intersect(controls));
            if (range.Count == 0) return;
            controls.RemoveAll(control => range.Contains(control));
            ControlCollectionChanged?.Invoke(this, ControlCollectionChangedEventArgs.Removed(range));
        }

        /// <summary>
        /// Gets an <see cref="IEnumerator{ConsoleControl}"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{ConsoleControl}"/> that can enumerate the <see cref="ConsoleControl"/>
        /// instances contained in this <see cref="ControlCollection"/>.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerator<ConsoleControl> GetEnumerator() => ((IEnumerable<ConsoleControl>)controls.ToArray()).GetEnumerator();
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
