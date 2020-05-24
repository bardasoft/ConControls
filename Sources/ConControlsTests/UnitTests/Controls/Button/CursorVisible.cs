/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void CursorVisible_AlwaysFalse()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow);
            sut.CursorVisible.Should().BeFalse();
            sut.CursorVisible = true;
            sut.CursorVisible.Should().BeFalse();
            sut.CursorVisible = false;
            sut.CursorVisible.Should().BeFalse();
        }
    }
}
