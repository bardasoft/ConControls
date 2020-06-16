/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using ConControls.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Extensions.CoordinateExtensions
{
    public partial class CoordinateExtensionsTests
    {
        [TestMethod]
        public void Pt_CorrectPointCreated()
        {
            const int x = 12;
            const int y = 23;

            var result = (x, y).Pt();
            result.X.Should().Be(x);
            result.Y.Should().Be(y);
        }
    }
}
