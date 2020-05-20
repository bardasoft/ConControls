/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Drawing;
using ConControls.Controls.Drawing;
using ConControls.Logging;

namespace ConControls.Controls 
{
    /// <summary>
    /// A console progress bar control.
    /// </summary>
    public sealed class ProgressBar : ConsoleControl
    {
        /// <summary>
        /// The default progress character used by this progress bar.
        /// </summary>
        public const char DefaultProgressChar = (char)0x2593;

        /// <summary>
        /// The orientation of the progress bar.
        /// </summary>
        public enum ProgressOrientation
        {
            /// <summary>
            /// Progress grows from left to right.
            /// </summary>
            LeftToRight,
            /// <summary>
            /// Progress grows from right to left.
            /// </summary>
            RightToLeft,
            /// <summary>
            /// Progress grows from top to bottom.
            /// </summary>
            TopToBottom,
            /// <summary>
            /// Progress grows from bottom to top.
            /// </summary>
            BottomToTop
        }

        double percentage;
        char progressChar = DefaultProgressChar;
        Rectangle filledRect;
        ProgressOrientation orientation = ProgressOrientation.LeftToRight;

        /// <summary>
        /// Raised when <see cref="Percentage"/> has been changed.
        /// </summary>
        public event EventHandler? PercentageChanged;
        /// <summary>
        /// Raised when <see cref="ProgressChar"/> has been changed.
        /// </summary>
        public event EventHandler? ProgressCharChanged;
        /// <summary>
        /// Raised when <see cref="Orientation"/> has been changed.
        /// </summary>
        public event EventHandler? OrientationChanged;

        /// <summary>
        /// Gets or sets the percentage value of this progress bar (between 0 and 1 inclusive).
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than zero or greater than 1.</exception>
        public double Percentage
        {
            get
            {
                lock (Window.SynchronizationLock) { return percentage; }
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (value < 0) throw Exceptions.ProgressBarPercentageMustBeNonNegative();
                    if (value > 1) throw Exceptions.ProgressBarPercentageMustNotBeGreaterThan1();
                    if (percentage.Equals(value)) return;
                    percentage = value;
                    OnPercentageChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the progress character used to display the progress bar.
        /// </summary>
        public char ProgressChar
        {
            get
            {
                lock (Window.SynchronizationLock) { return progressChar; }
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (progressChar == value) return;
                    progressChar = value;
                    OnProgressCharChanged();
                }
            }
        }
        /// <summary>
        /// Gets or sets the orientation of this progressbar (default is <see cref="ProgressOrientation.LeftToRight"/>).
        /// </summary>
        public ProgressOrientation Orientation
        {
            get
            {
                lock (Window.SynchronizationLock) { return orientation; }
            }
            set
            {
                lock (Window.SynchronizationLock)
                {
                    if (orientation == value) return;
                    orientation = value;
                    OnOrientationChanged();
                }
            }
        }
        /// <inheritdoc />
        public ProgressBar(IConsoleWindow window)
            : base(window) { }

        /// <inheritdoc />
        protected override void DrawClientArea(IConsoleGraphics graphics)
        {
            base.DrawClientArea(graphics);
            graphics.FillArea(EffectiveBackgroundColor, EffectiveForegroundColor, progressChar, GetRectangleToFill());
        }
        void UpdateRectangle()
        {
            Rectangle rect = GetRectangleToFill();
            if (rect == filledRect)
            {
                Logger.Log(DebugContext.ProgressBar, "Rectangle did not change, no redraw.");
                return;
            }
            Logger.Log(DebugContext.ProgressBar, "Rectangle changed... redrawing.");
            filledRect = rect;
            Draw();
        }
        Rectangle GetRectangleToFill()
        {
            var clientArea = GetClientArea();
            clientArea = new Rectangle(PointToConsole(clientArea.Location), clientArea.Size);
            return orientation switch
            {
                ProgressOrientation.RightToLeft => GetRightToLeftRectangle(clientArea),
                ProgressOrientation.TopToBottom => GetTopToBottomRectangle(clientArea),
                ProgressOrientation.BottomToTop => GetBottomToTopRectangle(clientArea),
                _ => GetLeftToRightRectangle(clientArea)
            };
        }
        Rectangle GetLeftToRightRectangle(Rectangle clientArea) =>
            new Rectangle(clientArea.Location, new Size((int)(percentage * clientArea.Width), clientArea.Height));
        Rectangle GetRightToLeftRectangle(Rectangle clientArea)
        {
            int x = (int)(percentage * clientArea.Width);
            return new Rectangle(clientArea.X + clientArea.Width - x, clientArea.Y, x, clientArea.Height);
        }
        Rectangle GetTopToBottomRectangle(Rectangle clientArea) =>
            new Rectangle(clientArea.Location, new Size(clientArea.Width, (int)(percentage * clientArea.Height)));
        Rectangle GetBottomToTopRectangle(Rectangle clientArea)
        {
            int y = (int)(percentage * clientArea.Height);
            return new Rectangle(clientArea.X, clientArea.Y + clientArea.Height - y, clientArea.Width, y);
        }

        void OnPercentageChanged()
        {
            UpdateRectangle();
            PercentageChanged?.Invoke(this, EventArgs.Empty);
        }
        void OnProgressCharChanged()
        {
            Draw();
            ProgressCharChanged?.Invoke(this, EventArgs.Empty);
        }
        void OnOrientationChanged()
        {
            UpdateRectangle();
            OrientationChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
