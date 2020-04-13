/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleOutputReceivedEventArgsTests
    {
        [TestMethod]
        public void ConsoleOutputReceivedEventArgs_ConstructorSetsCorrectValues()
        {
            const string msg = "msg";
            var sut = new ConsoleOutputReceivedEventArgs(msg);
            sut.Output.Should().Be(msg);
        }
    }
}
