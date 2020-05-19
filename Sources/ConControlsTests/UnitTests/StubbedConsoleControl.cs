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
using ConControls.Helpers;
using FluentAssertions;

namespace ConControlsTests.UnitTests 
{
    [ExcludeFromCodeCoverage]
    sealed class StubbedConsoleControl : ConsoleControl, IControlContainer
    {
        public bool? Focusable { get; set; }
        public Rectangle ClientArea => GetClientArea();
        public override bool CanFocus => Focusable ?? base.CanFocus;
        
        public void DoCheckDisposed() => CheckDisposed();
        public void DisposeInternal(bool disposing)
        {
            lock (Window.SynchronizationLock) Dispose(disposing);
        }
        public void DoDrawBackground(IConsoleGraphics graphics)
            => base.DrawBackground(graphics);
        public void DoDrawBorder(IConsoleGraphics graphics)
            => base.DrawBorder(graphics);
        public void DoDrawClientArea(IConsoleGraphics graphics)
            => base.DrawClientArea(graphics);

        public ConsoleColor EffForeColor => EffectiveForegroundColor;
        public ConsoleColor EffBackColor => EffectiveBackgroundColor;
        public ConsoleColor EffBorderColor => EffectiveBorderColor;
        public BorderStyle EffBorderStyle => EffectiveBorderStyle;

        readonly Dictionary<string, int> methodCallCounts = new Dictionary<string, int>();
        public int GetMethodCount(string method)
        {
            lock(methodCallCounts)
                return methodCallCounts.TryGetValue(method, out var count) ? count : 0;
        }
        public void ResetMethodCount()
        {
            lock(methodCallCounts) methodCallCounts.Clear();
        }

        internal StubbedConsoleControl()
            : this(null!) { }
        internal StubbedConsoleControl(IControlContainer parent)
            : base(parent)
        {
            deferrer = new DisposableBlock(() => OnDeferDrawingDisposed?.Invoke());
        }
        internal const string MethodDispose = nameof(Dispose);
        protected override void Dispose(bool disposing)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.Dispose(disposing);
            AddCount();
        }
        internal const string MethodDrawBackground = nameof(DrawBackground);
        protected override void DrawBackground(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawBackground(graphics);
            AddCount();
        }
        internal const string MethodDrawBorder= nameof(DrawBorder);
        protected override void DrawBorder(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawBorder(graphics);
            AddCount();
        }
        internal const string MethodDrawClientArea = nameof(DrawClientArea);
        protected override void DrawClientArea(IConsoleGraphics graphics)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.DrawClientArea(graphics);
            AddCount();
        }
        internal const string MethodOnParentChanged = nameof(OnParentChanged);
        protected override void OnParentChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnParentChanged();
            AddCount();
        }
        internal const string MethodOnNameChanged = nameof(OnNameChanged);
        protected override void OnNameChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnNameChanged();
            AddCount();
        }
        internal const string MethodOnCursorPositionChanged = nameof(OnCursorPositionChanged);
        protected override void OnCursorPositionChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorPositionChanged();
            AddCount();
        }
        internal const string MethodOnCursorSizeChanged = nameof(OnCursorSizeChanged);
        protected override void OnCursorSizeChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorSizeChanged();
            AddCount();
        }
        internal const string MethodOnCursorVisibleChanged = nameof(OnCursorVisibleChanged);
        protected override void OnCursorVisibleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnCursorVisibleChanged();
            AddCount();
        }
        internal const string MethodOnFocusedChanged = nameof(OnFocusedChanged);
        protected override void OnFocusedChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnFocusedChanged();
            AddCount();
        }
        internal const string MethodOnEnabledChanged = nameof(OnEnabledChanged);
        protected override void OnEnabledChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnEnabledChanged();
            AddCount();
        }
        internal const string MethodOnVisibleChanged = nameof(OnVisibleChanged);
        protected override void OnVisibleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnVisibleChanged();
            AddCount();
        }
        internal const string MethodOnForegroundColorChanged = nameof(OnForegroundColorChanged);
        protected override void OnForegroundColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnForegroundColorChanged();
            AddCount();
        }
        internal const string MethodOnBackgroundColorChanged = nameof(OnBackgroundColorChanged);
        protected override void OnBackgroundColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBackgroundColorChanged();
            AddCount();
        }
        internal const string MethodOnBorderColorChanged = nameof(OnBorderColorChanged);
        protected override void OnBorderColorChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBorderColorChanged();
            AddCount();
        }
        internal const string MethodOnBorderStyleChanged = nameof(OnBorderStyleChanged);
        protected override void OnBorderStyleChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnBorderStyleChanged();
            AddCount();
        }
        internal const string MethodOnAreaChanged = nameof(OnAreaChanged);
        protected override void OnAreaChanged()
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnAreaChanged();
            AddCount();
        }
        internal const string MethodOnKeyEvent = nameof(OnKeyEvent);
        protected override void OnKeyEvent(object sender, KeyEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnKeyEvent(sender, e);
            AddCount();
        }
        internal const string MethodOnMouseEvent = nameof(OnMouseEvent);
        protected override void OnMouseEvent(object sender, MouseEventArgs e)
        {
            Monitor.IsEntered(Window.SynchronizationLock).Should().BeTrue();
            base.OnMouseEvent(sender, e);
            AddCount();
        }

        void AddCount([CallerMemberName] string caller = "")
        {
            lock (methodCallCounts)
                methodCallCounts[caller] = methodCallCounts.TryGetValue(caller, out int v)
                                               ? v + 1
                                               : 1;
        }
        public event Action? OnDeferDrawingDisposed;
        readonly DisposableBlock deferrer;
        IDisposable IControlContainer.DeferDrawing()
        {
            return deferrer;
        }
    }
}
