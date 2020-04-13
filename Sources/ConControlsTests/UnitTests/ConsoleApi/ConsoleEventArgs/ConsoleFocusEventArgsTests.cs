/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.ConsoleApi;
using ConControls.WindowsApi.Types;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleEventArgs
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConsoleFocusEventArgsTests
    {
        [TestMethod]
        public void ConsoleFocusEventArgs_ConstructorSetsCorrectValues()
        {
            FOCUS_EVENT_RECORD record = new FOCUS_EVENT_RECORD
            {
                SetFocus = 12
            };
            var sut = new ConsoleFocusEventArgs(record);
            sut.SetFocus.Should().BeTrue();
            record = new FOCUS_EVENT_RECORD
            {
                SetFocus = 0
            };
            sut = new ConsoleFocusEventArgs(record);
            sut.SetFocus.Should().BeFalse();
        }
    }
}
