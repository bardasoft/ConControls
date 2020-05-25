/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.ConsoleWindow
{
    public partial class ConsoleWindowTests
    {
        [TestMethod]
        public void FocusedControl_NonFocussableControl_InvalidOperationException()
        {
            var api = new StubbedNativeCalls();
            using var controller = new StubbedConsoleController();
            var graphicsProvider = new StubbedGraphicsProvider();

            using var sut = new ConControls.Controls.ConsoleWindow(api, controller, graphicsProvider);
            using var c = new Panel(sut) {Parent = sut};
            sut.Invoking(s => s.FocusedControl = c).Should().Throw<InvalidOperationException>();
        }
    }
}
