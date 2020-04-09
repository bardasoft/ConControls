/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls;
using ConControls.ConsoleApi;
using ConControls.Controls;

#nullable enable

namespace ConControlsTests.Stubs
{
    [ExcludeFromCodeCoverage]
    class StubIConsoleWindow : IConsoleWindow
    {
        public Action? Dispose { get; set; }
        void IDisposable.Dispose() => Dispose?.Invoke();
        public Action<EventHandler?>? AddSizeChanged { get; set; }
        public Action<EventHandler?>? RemoveSizeChanged { get; set; }
        event EventHandler? IConsoleWindow.SizeChanged
        {
            add => AddSizeChanged?.Invoke(value);
            remove => RemoveSizeChanged?.Invoke(value);
        }
        public Action<EventHandler?>? AddDisposed { get; set; }
        public Action<EventHandler?>? RemoveDisposed { get; set; }
        event EventHandler? IConsoleWindow.Disposed
        {
            add => AddDisposed?.Invoke(value);
            remove => RemoveDisposed?.Invoke(value);
        }
        public Func<string>? GetTitle { get; set; }
        public Action<string>? SetTitle { get; set; }
        string IConsoleWindow.Title
        {
            get => GetTitle?.Invoke() ?? string.Empty;
            set => SetTitle?.Invoke(value);
        }
        public Func<Size>? GetSize { get; set; }
        public Action<Size>? SetSize { get; set; }
        Size IConsoleWindow.Size
        {
            get => GetSize?.Invoke() ?? default;
            set => SetSize?.Invoke(value);
        }
        public Func<Size>? GetMaximumSize { get; set; }
        Size IConsoleWindow.MaximumSize => GetMaximumSize?.Invoke() ?? default;
        public Func<ConsoleColor>? GetBackgroundColor { get; set; }
        public Action<ConsoleColor>? SetBackgroundColor{ get; set; }
        ConsoleColor IConsoleWindow.BackgroundColor
        {
            get => GetBackgroundColor?.Invoke() ?? default;
            set => SetBackgroundColor?.Invoke(value);
        }
        public Func<ControlCollection>? GetControls { get; set; }
        ControlCollection IConsoleWindow.Controls => GetControls?.Invoke() ?? throw new NotImplementedException();
        public Func<FrameCharSets>? GetFrameCharSets{ get; set; }
        public Action<FrameCharSets>? SetFrameCharSets{ get; set; }
        FrameCharSets IConsoleWindow.FrameCharSets
        {
            get => GetFrameCharSets?.Invoke() ?? throw new NotImplementedException();
            set => SetFrameCharSets?.Invoke(value);
        }
        public Func<bool>? GetDrawingInhibited { get; set; }
        bool IConsoleWindow.DrawingInhibited => GetDrawingInhibited?.Invoke() ?? false;
        public Func<bool>? GetIsDisposed { get; set; }
        bool IConsoleWindow.IsDisposed => GetIsDisposed?.Invoke() ?? false;
        public Func<object>? GetSynchronizationLock { get; set; }
        object IConsoleWindow.SynchronizationLock => GetSynchronizationLock?.Invoke() ?? throw new NotImplementedException();
        public Func<IConsoleGraphics>? GetGraphics { get; set; }
        IConsoleGraphics IConsoleWindow.GetGraphics() => GetGraphics?.Invoke() ?? throw new NotImplementedException();
        public Action? Draw { get; set; }
        void IConsoleWindow.Draw() => Draw?.Invoke();
        public Action? Refresh { get; set; }
        void IConsoleWindow.Refresh() => Refresh?.Invoke();
        public Action? BeginUpdate { get; set; }
        void IConsoleWindow.BeginUpdate() => BeginUpdate?.Invoke();
        public Action? EndUpdate { get; set; }
        void IConsoleWindow.EndUpdate() => EndUpdate?.Invoke();
    }
}
