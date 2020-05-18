/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using ConControls.Controls.Drawing.Fakes;
using ConControls.Controls.Fakes;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void PointToClient_CorrectResult()
        {
            var stubbedWindow = new StubbedWindow
            {
                PointToClientPoint = p => p,
                PointToConsolePoint = p => p
            };

            var l1 = new Point(12, 34);
            var l2 = new Point(23, 42);

            var sut1 = new Panel(stubbedWindow)
            {
                Location = l1
            };
            var sut2 = new Panel(sut1)
            {
                Location = l2
            };

            var consolePoint = new Point(123, 456);
            sut2.PointToClient(consolePoint)
                .Should()
                .Be(new Point(
                        consolePoint.X - l1.X - l2.X, 
                        consolePoint.Y - l1.Y - l2.Y));
        }
    }
}
