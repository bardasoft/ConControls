/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void CheckDisposed_NotDisposed_Nothing()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DoCheckDisposed();
        }
        [TestMethod]
        public void CheckDisposed_WindowDisposed_ObjectDisposedException()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            stubbedWindow.IsDisposedGet = () => true;
            sut.Invoking(c => c.DoCheckDisposed()).Should().Throw<ObjectDisposedException>().Which.ObjectName.Should().Be(nameof(ConsoleWindow));
        }
        [TestMethod]
        public void CheckDisposed_ControlDisposed_ObjectDisposedException()
        {
            var stubbedWindow = new StubbedWindow();
            var sut = new StubbedConsoleControl(stubbedWindow);
            sut.DisposeInternal(false);
            stubbedWindow.KeyEventEvent.Should().NotBeNull();
            stubbedWindow.MouseEventEvent.Should().NotBeNull();
            sut.Dispose();
            sut.Invoking(c => c.DoCheckDisposed())
               .Should().Throw<ObjectDisposedException>()
               .Which
               .ObjectName
               .Should().Be(nameof(StubbedConsoleControl));
        }
    }
}
