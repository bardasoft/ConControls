/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using ConControls.Controls;
using ConControls.Controls.Drawing;
using FluentAssertions;

namespace ConControlsTests.UnitTests.Controls 
{
    [ExcludeFromCodeCoverage]
    sealed class TestControl : ConControls.Controls.ConsoleControl
    {
        public bool? Focusable { get; set; }
        public Rectangle ClientArea => GetClientArea();
        public override bool CanFocus => Focusable ?? base.CanFocus;
        public void DoCheckDisposed() => CheckDisposed();
        public void DisposeInternal(bool disposing)
        {
            lock (Window.SynchronizationLock) Dispose(disposing);
        }

        public ConsoleColor EffForeColor => EffectiveForegroundColor;
        public ConsoleColor EffBackColor => EffectiveBackgroundColor;
        public ConsoleColor EffBorderColor => EffectiveBorderColor;
        public BorderStyle EffBorderStyle => EffectiveBorderStyle;

        public Dictionary<string, int> MethodCallCounts { get; } = new Dictionary<string, int>();
        internal TestControl()
            : base(null!) { }
        internal TestControl(IControlContainer parent)
            : base(parent) { }
        protected override void Dispose(bool disposing)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.Dispose(disposing);
            AddCount();
        }
        /// <inheritdoc />
        protected override void Draw()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.Draw();
            AddCount();
        }
        /// <inheritdoc />
        protected override void DrawBackground(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawBackground(graphics);
            AddCount();
        }
        /// <inheritdoc />
        protected override void DrawBorder(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawBorder(graphics);
            AddCount();
        }
        /// <inheritdoc />
        protected override void DrawClientArea(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawClientArea(graphics);
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnParentChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnParentChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnNameChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnNameChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnCursorPositionChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorPositionChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnCursorSizeChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorSizeChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnCursorVisibleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorVisibleChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnFocusedChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnFocusedChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnEnabledChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnEnabledChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnVisibleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnVisibleChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnForegroundColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnForegroundColorChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnBackgroundColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBackgroundColorChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnBorderColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBorderColorChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnBorderStyleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBorderStyleChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnAreaChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnAreaChanged();
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnKeyEvent(object sender, KeyEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnKeyEvent(sender, e);
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnMouseEvent(object sender, MouseEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnMouseEvent(sender, e);
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnStdOutEvent(object sender, StdOutEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnStdOutEvent(sender, e);
            AddCount();
        }
        /// <inheritdoc />
        protected override void OnStdErrEvent(object sender, StdErrEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnStdErrEvent(sender, e);
            AddCount();
        }

        void AddCount([CallerMemberName] string caller = "")
        {
            lock (MethodCallCounts)
                MethodCallCounts[caller] = MethodCallCounts.TryGetValue(caller, out int v)
                                               ? v + 1
                                               : 1;
        }
    }
}
