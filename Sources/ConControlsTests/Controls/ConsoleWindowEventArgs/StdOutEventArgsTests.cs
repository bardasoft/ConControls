/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.Controls.ConsoleWindowEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StdOutEventArgsTests
    {
        [TestMethod]
        public void StdOutEventArgs_ConstructorSetsCorrectValues()
        {
            const string msg = "msg";
            var sut = new StdOutEventArgs(new ConsoleOutputReceivedEventArgs(msg));
            sut.Output.Should().Be(msg);
        }
    }
}
