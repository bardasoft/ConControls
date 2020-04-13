/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleSizeEventArgsTests
    {
        [TestMethod]
        public void ConsoleSizeEventArgs_ConstructorSetsCorrectValues()
        {
            WINDOW_BUFFER_SIZE_RECORD record = new WINDOW_BUFFER_SIZE_RECORD
            {
                Size = new COORD(21, 42)
            };
            var sut = new ConsoleSizeEventArgs(record);
            sut.Size.Width.Should().Be(21);
            sut.Size.Height.Should().Be(42);
        }
    }
}
