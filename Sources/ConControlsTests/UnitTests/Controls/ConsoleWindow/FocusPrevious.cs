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
        public void FocusPrevious_Empty_Null()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            sut.FocusedControl.Should().BeNull();
            sut.FocusPrevious().Should().BeNull();
            sut.FocusPrevious().Should().BeNull();
        }
        [TestMethod]
        public void FocusPrevious_Single_Focused()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var c0 = new Panel(sut){Parent = sut};
            var f00 = new ConControls.Controls.TextBlock(sut) {Parent = c0};
            sut.Controls.Add(new Panel(sut));

            sut.FocusedControl.Should().BeNull();
            sut.FocusPrevious().Should().Be(f00);
            sut.FocusPrevious().Should().Be(f00);
            sut.FocusPrevious().Should().Be(f00);
        }
        [TestMethod]
        public void FocusPrevious_SingleThenDisabled_Null()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var f0 = new ConControls.Controls.TextBlock(sut) {Parent = sut};
            sut.FocusedControl.Should().BeNull();
            sut.FocusPrevious().Should().Be(f0);
            f0.Enabled = false;
            sut.FocusPrevious().Should().BeNull();
        }
        [TestMethod]
        public void FocusPrevious_Cousins_CorrectOrder()
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
            sut.FocusPrevious().Should().Be(f10);
            sut.FocusPrevious().Should().Be(f00);
            sut.FocusPrevious().Should().Be(f10);
            sut.FocusPrevious().Should().Be(f00);
        }
        [TestMethod]
        public void FocusPrevious_MultipleTabOrder_CorrectSequence()
        {
            using var consoleController = new StubbedConsoleController();
            using var api = new StubbedNativeCalls();
            var graphicsProvider = new StubbedGraphicsProvider();
            using var sut = new ConControls.Controls.ConsoleWindow(api, consoleController, graphicsProvider);

            var c0 = new Panel(sut) { Parent = sut, TabOrder = 10 };
            var f00 = new ConControls.Controls.TextBlock(sut) { Parent = c0, TabOrder = 20 };
            var f01 = new ConControls.Controls.TextBlock(sut) { Parent = c0, TabOrder = 10 };
            var c02 = new Panel(sut) { Parent = c0, TabOrder = 15 };
            var f020 = new ConControls.Controls.TextBlock(sut){ Parent = c02};
            c02.Controls.Add(new ConControls.Controls.TextBlock(sut) {Enabled = false });
            var c1 = new Panel(sut) { Parent = sut, TabOrder = 5 };
            var f10 = new ConControls.Controls.TextBlock(sut){ Parent = c1};
            var f100 = new ConControls.Controls.TextBlock(sut) { Parent = f10, TabOrder = 10 };
            var f101 = new ConControls.Controls.TextBlock(sut) { Parent = f10, TabOrder = 10 };

            sut.FocusedControl.Should().BeNull();
            sut.FocusPrevious().Should().Be(f00);
            sut.FocusPrevious().Should().Be(f020);
            sut.FocusPrevious().Should().Be(f01);
            sut.FocusPrevious().Should().Be(f101);
            sut.FocusPrevious().Should().Be(f100);
            sut.FocusPrevious().Should().Be(f10);
            sut.FocusPrevious().Should().Be(f00);
            sut.FocusPrevious().Should().Be(f020);
            sut.FocusPrevious().Should().Be(f01);
            sut.FocusPrevious().Should().Be(f101);
            sut.FocusPrevious().Should().Be(f100);
            sut.FocusPrevious().Should().Be(f10);
        }
    }
}
