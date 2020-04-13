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
    public class StdErrEventArgsTests
    {
        [TestMethod]
        public void StdErrEventArgs_ConstructorSetsCorrectValues()
        {
            const string msg = "msg";
            var sut = new StdErrEventArgs(new ConsoleOutputReceivedEventArgs(msg));
            sut.Error.Should().Be(msg);
        }
    }
}
