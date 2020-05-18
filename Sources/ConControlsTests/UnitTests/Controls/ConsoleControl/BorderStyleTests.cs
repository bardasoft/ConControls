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

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void BorderStyle_Changed_ThreadSafeHandlerCall()
        {
            var stubbedWindow = new StubbedWindow
            {
                BorderStyleGet = () => BorderStyle.SingleLined
            };

            var sut = new TestControl(stubbedWindow);
            sut.BorderStyle.Should().Be(BorderStyle.SingleLined);

            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.BorderStyle = BorderStyle.DoubleLined;
            sut.BorderStyle.Should().Be(BorderStyle.DoubleLined);
            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(1);
        }
        [TestMethod]
        public void BorderStyle_NotChanged_NoEvent()
        {
            var stubbedWindow = new StubbedWindow
            {
                BorderStyleGet = () => BorderStyle.SingleLined
            };

            var sut = new TestControl(stubbedWindow);

            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
            sut.BorderStyle = sut.BorderStyle;
            sut.BorderStyle.Should().Be(BorderStyle.SingleLined);
            sut.GetMethodCount(TestControl.MethodOnBorderStyleChanged).Should().Be(0);
        }
    }
}
