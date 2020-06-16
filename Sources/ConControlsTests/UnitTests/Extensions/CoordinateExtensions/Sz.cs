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
        public void Sz_CorrectSizeCreated()
        {
            const int width = 12;
            const int height = 23;

            var result = (width, height).Sz();
            result.Width.Should().Be(width);
            result.Height.Should().Be(height);
        }
    }
}
