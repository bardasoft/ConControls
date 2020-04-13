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
    public class ConsoleMenuEventArgsTests
    {
        [TestMethod]
        public void ConsoleMenuEventArgs_ConstructorSetsCorrectValues()
        {
            MENU_EVENT_RECORD record = new MENU_EVENT_RECORD
            {
                CommandId = 12
            };
            var sut = new ConsoleMenuEventArgs(record);
            sut.CommandId.Should().Be(12);
        }
    }
}
