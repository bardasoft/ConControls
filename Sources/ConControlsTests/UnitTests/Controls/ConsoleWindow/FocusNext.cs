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

            var c0 = new Panel(sut) {Parent = sut};
            var f00 = new ConControls.Controls.TextBlock(sut) {Parent = c0};
            sut.Controls.Add(new Panel(sut));

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

            var f0 = new ConControls.Controls.TextBlock(sut) {Parent = sut};
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

            var c0 = new Panel(sut) {Parent = sut};
            var f00 = new ConControls.Controls.TextBlock(sut) {Parent = c0};
            var c1 = new Panel(sut) {Parent = sut};
            var f10 = new ConControls.Controls.TextBlock(sut) {Parent = c1};

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
            var f00 = new ConControls.Controls.TextBlock(sut) {TabOrder = 20};
            var f01 = new ConControls.Controls.TextBlock(sut) {TabOrder = 10};
            var c02 = new Panel(sut) {TabOrder = 15};
            var f020 = new ConControls.Controls.TextBlock(sut);
            var c021 = new ConControls.Controls.TextBlock(sut) {Enabled = false};
            var c1 = new Panel(sut) {TabOrder = 5};
            var f10 = new ConControls.Controls.TextBlock(sut);
            var f100 = new ConControls.Controls.TextBlock(sut) { TabOrder = 10 };
            var f101 = new ConControls.Controls.TextBlock(sut) { TabOrder = 10 };

            sut.Controls.AddRange(c0, c1);
            
            c0.Controls.AddRange(f00, f01, c02);
            c02.Controls.AddRange(f020, c021);

            c1.Controls.Add(f10);
            f10.Controls.AddRange(f100, f101);

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
