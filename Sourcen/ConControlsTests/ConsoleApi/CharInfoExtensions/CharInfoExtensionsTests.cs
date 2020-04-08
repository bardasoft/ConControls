/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.ConsoleApi.CharInfoExtensions
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class CharInfoExtensionsTests
    {
        [TestMethod]
        public void ToForegroundColor()
        {
            ConsoleColor.Black.ToForegroundColor().Should().Be(ConCharAttributes.None);
            ConsoleColor.DarkBlue.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue);
            ConsoleColor.DarkGreen.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundGreen);
            ConsoleColor.DarkCyan.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen);
            ConsoleColor.DarkRed.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundRed);
            ConsoleColor.DarkMagenta.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundRed);
            ConsoleColor.DarkYellow.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed);
            ConsoleColor.Gray.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed);
            ConsoleColor.DarkGray.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Blue.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Green.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Cyan.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Red.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Magenta.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.Yellow.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity);
            ConsoleColor.White.ToForegroundColor().Should().Be(ConCharAttributes.ForeGroundBlue | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundRed | ConCharAttributes.ForeGroundIntensity);
            ((ConsoleColor)255).Invoking(c => c.ToForegroundColor())
                               .Should()
                               .Throw<ArgumentOutOfRangeException>()
                               .Which.ActualValue.Should()
                               .BeEquivalentTo(255);
        }
        [TestMethod]
        public void ToBackgroundColor()
        {
            ConsoleColor.Black.ToBackgroundColor().Should().Be(ConCharAttributes.None);
            ConsoleColor.DarkBlue.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue);
            ConsoleColor.DarkGreen.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundGreen);
            ConsoleColor.DarkCyan.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen);
            ConsoleColor.DarkRed.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundRed);
            ConsoleColor.DarkMagenta.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundRed);
            ConsoleColor.DarkYellow.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed);
            ConsoleColor.Gray.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed);
            ConsoleColor.DarkGray.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Blue.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Green.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Cyan.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Red.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Magenta.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.Yellow.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity);
            ConsoleColor.White.ToBackgroundColor().Should().Be(ConCharAttributes.BackGroundBlue | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundRed | ConCharAttributes.BackGroundIntensity);
            ((ConsoleColor)255).Invoking(c => c.ToBackgroundColor())
                               .Should()
                               .Throw<ArgumentOutOfRangeException>()
                               .Which.ActualValue.Should()
                               .BeEquivalentTo(255);
        }
        [TestMethod]
        public void SetBackground()
        {
            const ConCharAttributes attribute = (ConCharAttributes)0xCCCC;
            const ConCharAttributes expected = (ConCharAttributes)0xCC0C | ConCharAttributes.BackGroundGreen | ConCharAttributes.BackGroundIntensity;
            CHAR_INFO source = new CHAR_INFO('x', attribute);
            var result = source.SetBackground(ConsoleColor.Green);
            result.Char.Should().Be('x');
            result.Attributes.Should().Be(expected);
        }
        [TestMethod]
        public void Setforeground()
        {
            const ConCharAttributes attribute = (ConCharAttributes)0xCCCC;
            const ConCharAttributes expected = (ConCharAttributes)0xCCC0 | ConCharAttributes.ForeGroundGreen | ConCharAttributes.ForeGroundIntensity;
            CHAR_INFO source = new CHAR_INFO('x', attribute);
            var result = source.SetForeground(ConsoleColor.Green);
            result.Char.Should().Be('x');
            result.Attributes.Should().Be(expected);
        }
        [TestMethod]
        public void SetChar()
        {
            const ConCharAttributes attribute = (ConCharAttributes)0xCCCC;
            CHAR_INFO source = new CHAR_INFO(' ', attribute);
            var result = source.SetChar('x');
            result.Char.Should().Be('x');
            result.Attributes.Should().Be(attribute);
        }
    }
}
