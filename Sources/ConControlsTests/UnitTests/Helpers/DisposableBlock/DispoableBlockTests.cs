/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Helpers.DisposableBlock
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DispoableBlockTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DisposableBock_Null_ArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new ConControls.Helpers.DisposableBlock(null!);
        }
        [TestMethod]
        public void DisposableBock_CallsDisposeAction()
        {
            bool actionCalled = false;
            void action() => actionCalled = true;
            var sut = new ConControls.Helpers.DisposableBlock(action);
            actionCalled.Should().BeFalse();
            sut.Dispose();
            actionCalled.Should().BeTrue();
        }
    }
}
