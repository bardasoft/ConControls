/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void Title_GetAndSetProperly()
        {
            string title = string.Empty;
            var api = new StubbedNativeCalls
            {
                GetConsoleTitle = () => title,
                SetConsoleTitleString = s => title = s
            };
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);

            sut.Title.Should().Be(string.Empty);
            sut.Title = "hello";
            title.Should().Be("hello");
            sut.Title.Should().Be("hello");
            sut.Title = "world";
            title.Should().Be("world");
            sut.Title.Should().Be("world");

            sut.Title = null!;
            title.Should().BeEmpty();
            sut.Title.Should().BeEmpty();
        }
    }
}
