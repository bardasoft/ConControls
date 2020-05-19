/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void FocusNext_Empty_Null()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            sut.FocusedControl.Should().BeNull();
            sut.FocusNext().Should().BeNull();
            sut.FocusNext().Should().BeNull();
        }
        [TestMethod]
        public void FocusNext_Single_Focused()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var c0 = new Panel(sut);
            var f00 = new TextBlock(c0);
            _ = new Panel(sut);

            sut.FocusedControl.Should().BeNull();
            sut.FocusNext().Should().Be(f00);
            sut.FocusNext().Should().Be(f00);
            sut.FocusNext().Should().Be(f00);
        }
        [TestMethod] 
        public void FocusNext_SingleThenDisabled_Null()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var f0 = new TextBlock(sut);
            sut.FocusedControl.Should().BeNull();
            sut.FocusNext().Should().Be(f0);
            f0.Enabled = false;
            sut.FocusNext().Should().BeNull();
        }
        [TestMethod]
        public void FocusNext_Cousins_CorrectOrder()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var c0 = new Panel(sut);
            var f00 = new TextBlock(c0);
            var c1 = new Panel(sut);
            var f10 = new TextBlock(c1);

            sut.FocusedControl.Should().BeNull();
            sut.FocusNext().Should().Be(f00);
            sut.FocusNext().Should().Be(f10);
            sut.FocusNext().Should().Be(f00);
            sut.FocusNext().Should().Be(f10);
        }
        [TestMethod]
        public void FocusNext_MultipleTabOrder_CorrectSequence()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var c0 = new Panel(sut) {TabOrder = 10};
            var f00 = new TextBlock(c0) {TabOrder = 20};
            var f01 = new TextBlock(c0) {TabOrder = 10};
            var c02 = new Panel(c0) {TabOrder = 15};
            var f020 = new TextBlock(c02);
            _ = new TextBlock(c02) {Enabled = false};
            var c1 = new Panel(sut) {TabOrder = 5};
            var f10 = new TextBlock(c1);
            var f100 = new TextBlock(f10) { TabOrder = 10 };
            var f101 = new TextBlock(f10) { TabOrder = 10 };

            sut.FocusedControl.Should().BeNull();
            sut.FocusNext().Should().Be(f10);
            sut.FocusNext().Should().Be(f100);
            sut.FocusNext().Should().Be(f101);
            sut.FocusNext().Should().Be(f01);
            sut.FocusNext().Should().Be(f020);
            sut.FocusNext().Should().Be(f00);
            sut.FocusNext().Should().Be(f10);
            sut.FocusNext().Should().Be(f100);
            sut.FocusNext().Should().Be(f101);
            sut.FocusNext().Should().Be(f01);
            sut.FocusNext().Should().Be(f020);
            sut.FocusNext().Should().Be(f00);
        }
    }
}
