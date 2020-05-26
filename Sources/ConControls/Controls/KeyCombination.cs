/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.WindowsApi.Types;

namespace ConControls.Controls
{
    /// <summary>
    /// Represents a key combination with modifier keys.
    /// </summary>
    public readonly struct KeyCombination : IEquatable<KeyCombination>
    {
        /// <summary>
        /// Escape.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static KeyCombination Escape { get; } = new KeyCombination(VirtualKey.Escape);
        /// <summary>
        /// Alt + F4.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static KeyCombination AltF4 { get; } = new KeyCombination(VirtualKey.F4).WithAlt();
        /// <summary>
        /// F11.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static KeyCombination F11 { get; } = new KeyCombination(VirtualKey.F11);
        /// <summary>
        /// Gets wether an Alt key needs to be pressed.
        /// </summary>
        public bool Alt { get; }
        /// <summary>
        /// Gets wether a Control key needs to be pressed.
        /// </summary>
        public bool Ctrl { get; }
        /// <summary>
        /// Gets wether a shiftkey needs to be pressed.
        /// </summary>
        public bool Shift { get; }
        /// <summary>
        /// Gets the <see cref="VirtualKey"/> of this <see cref="KeyCombination"/>.
        /// </summary>
        public VirtualKey Key { get; }
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the given <paramref name="key"/> and no modifier keys.
        /// </summary>
        /// <param name="key">The key for the new <see cref="KeyCombination"/>.</param>
        public KeyCombination(VirtualKey key) => (Key, Alt, Ctrl, Shift) = (key, false, false, false);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the given <paramref name="key"/> and modifier keys.
        /// </summary>
        /// <param name="key">The key for the new <see cref="KeyCombination"/>.</param>
        /// <param name="alt"><c>true</c> if an Alt key should be pressed for this combination.</param>
        /// <param name="ctrl"><c>true</c> if a Control key should be pressed for this combination.</param>
        /// <param name="shift"><c>true</c> if a shift key should be pressed for this combination.</param>
        public KeyCombination(VirtualKey key, bool alt, bool ctrl, bool shift) : this(key) => (Alt, Ctrl, Shift) = (alt, ctrl, shift);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but an Alt key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but see <see cref="Alt"/> pressed.</returns>
        public KeyCombination WithAlt() => new KeyCombination(Key, true, Ctrl, Shift);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but a Control key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but see <see cref="Ctrl"/> pressed.</returns>
        public KeyCombination WithCtrl() => new KeyCombination(Key, Alt, true, Shift);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but a shift key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but see <see cref="Shift"/> pressed.</returns>
        public KeyCombination WithShift() => new KeyCombination(Key, Alt, Ctrl, true);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but no Alt key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but not see <see cref="Alt"/> pressed.</returns>
        public KeyCombination WithoutAlt() => new KeyCombination(Key, false, Ctrl, Shift);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but no Control key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but not see <see cref="Ctrl"/> pressed.</returns>
        public KeyCombination WithoutCtrl() => new KeyCombination(Key, Alt, false, Shift);
        /// <summary>
        /// Creates a new <see cref="KeyCombination"/> with the current settings but no shift key pressed.
        /// </summary>
        /// <returns>A new <see cref="KeyCombination"/> with the same values as the current instance but not see <see cref="Shift"/> pressed.</returns>
        public KeyCombination WithoutShift() => new KeyCombination(Key, Alt, Ctrl, false);

        /// <summary>
        /// Tests a value or reference for equality.
        /// </summary>
        /// <param name="obj">The instance to check for equality.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="KeyCombination"/> and has the same properties as the current instance.</returns>
        public override bool Equals(object obj) => obj is KeyCombination kc && this == kc;

        /// <summary>
        /// Tests a <see cref="KeyCombination"/> instance for equality.
        /// </summary>
        /// <param name="other">The other <see cref="KeyCombination"/> to check for equality.</param>
        /// <returns><c>true</c> if <paramref name="other"/> has the same properties as the current instance.</returns>
        public bool Equals(KeyCombination other) => this == other;
        /// <summary>
        /// Calculates a hash code for this instance.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Alt.GetHashCode();
                hashCode = (hashCode * 397) ^ Ctrl.GetHashCode();
                hashCode = (hashCode * 397) ^ Shift.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Key;
                return hashCode;
            }
        }
        /// <summary>
        /// Tests two <see cref="KeyCombination"/> instances for equality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the two instances have the same property values, <c>false</c> if not.</returns>
        public static bool operator ==(KeyCombination left, KeyCombination right) =>
            left.Key == right.Key &&
            left.Alt == right.Alt &&
            left.Ctrl == right.Ctrl &&
            left.Shift == right.Shift;

        /// <summary>
        /// Tests two <see cref="KeyCombination"/> instances for inequality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>false</c> if the two instances have the same property values, <c>true</c> if not.</returns>
        public static bool operator !=(KeyCombination left, KeyCombination right)
        {
            return !(left == right);
        }
    }
}
