/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public async Task WaitForCloseAsync_Works()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, new StubbedGraphicsProvider());
            sut.Close(12);
            (await sut.WaitForCloseAsync()).Should().Be(12);

        }
    }
}
