/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System.Diagnostics.CodeAnalysis;
using ConControls.Controls;
using ConControls.Controls.Drawing;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.Controls.Drawing.FrameCharSets
{
    [TestClass]
    [ExcludeFromCodeCoverage]

    public class FrameCharSetsTests
    {
        [TestMethod]
        public void FrameCharSetsIndexer_SingleLinedStyle_SingleLinedSet()
        {
            var sut = new ConControls.Controls.Drawing.FrameCharSets();
            sut[BorderStyle.SingleLined].Should().BeOfType<SingleLinedFrameCharSet>();
        }
        [TestMethod]
        public void FrameCharSetsIndexer_DoubleLinedStyle_DoubleLinedSet()
        {
            var sut = new ConControls.Controls.Drawing.FrameCharSets();
            sut[BorderStyle.DoubleLined].Should().BeOfType<DoubleLinedFrameCharSet>();
        }
        [TestMethod]
        public void FrameCharSetsIndexer_BoldLinedStyle_BoldLinedSet()
        {
            var sut = new ConControls.Controls.Drawing.FrameCharSets();
            sut[BorderStyle.Bold].Should().BeOfType<BoldinedFrameCharSet>();
        }
        [TestMethod]
        public void FrameCharSetsIndexer_UndefinedStyle_SingleLinedSet()
        {
            var sut = new ConControls.Controls.Drawing.FrameCharSets();
            sut[(BorderStyle)0x7FFFFFF].Should().BeOfType<SingleLinedFrameCharSet>();
        }
    }
}
