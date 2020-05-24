/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void Constructor_Initialized()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow);
            sut.BackgroundColor.Should().Be(ConsoleColor.DarkBlue);
            sut.ForegroundColor.Should().Be(ConsoleColor.DarkYellow);
            sut.DisabledBackgroundColor.Should().Be(ConsoleColor.DarkBlue);
            sut.DisabledForegroundColor.Should().Be(ConsoleColor.Gray);
            sut.BorderColor.Should().Be(ConsoleColor.DarkYellow);
            sut.BorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.FocusedBackgroundColor.Should().Be(ConsoleColor.Blue);
            sut.FocusedForegroundColor.Should().Be(ConsoleColor.Yellow);
            sut.FocusedBorderColor.Should().Be(ConsoleColor.Yellow);
            sut.FocusedBorderStyle.Should().Be(BorderStyle.DoubleLined);
        }
    }
}
