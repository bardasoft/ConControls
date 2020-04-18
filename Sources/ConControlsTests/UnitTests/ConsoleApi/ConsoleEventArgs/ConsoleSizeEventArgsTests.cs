/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using ConControls.ConsoleApi;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleSizeEventArgsTests
    {
        [TestMethod]
        public void ConsoleSizeEventArgs_ConstructorSetsCorrectValues()
        {
            Rectangle window = new Rectangle(1, 2, 3, 4);
            Size buffer = new Size(5, 6);
            var sut = new ConsoleSizeEventArgs(window, buffer);
            sut.WindowArea.Should().Be(window);
            sut.BufferSize.Should().Be(buffer);
        }
    }
}
