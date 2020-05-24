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
// ReSharper disable AccessToDisposedClosure

namespace ConControlsTests.UnitTests.Controls.TextControl
{
    public partial class TextControlTests
    {
        [TestMethod]
        public void MouseEvents_EventArgsNull_ArgumentNullException()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new StubbedTextControl(stubbedWindow);
            sut.Invoking(s => stubbedWindow.MouseEventEvent(stubbedWindow, null!)).Should().Throw<ArgumentNullException>();
        }
    }
}
