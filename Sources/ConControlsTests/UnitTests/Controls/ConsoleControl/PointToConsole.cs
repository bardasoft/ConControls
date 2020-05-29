/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Drawing;
using ConControls.Controls;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.Controls.ConsoleControl
{
    public partial class ConsoleControlTests
    {
        [TestMethod]
        public void PointToConsole_WithoutBorders_CorrectResult()
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
                Location = l1,
                Parent = stubbedWindow
            };
            var sut2 = new Panel(stubbedWindow)
            {
                Location = l2,
                Parent = sut1
            };

            var clientPoint = new Point(123, 456);
            sut2.PointToConsole(clientPoint)
                .Should()
                .Be(new Point(
                        clientPoint.X + l1.X + l2.X,
                        clientPoint.Y + l1.Y + l2.Y));

        }
        [TestMethod]
        public void PointToConsole_WithBorders_CorrectResult()
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
                Location = l1,
                Parent = stubbedWindow,
                BorderStyle = BorderStyle.SingleLined
            };
            var sut2 = new Panel(stubbedWindow)
            {
                Location = l2,
                Parent = sut1
            };

            var clientPoint = new Point(123, 456);
            sut2.PointToConsole(clientPoint)
                .Should()
                .Be(new Point(
                        clientPoint.X + l1.X + l2.X + 1,
                        clientPoint.Y + l1.Y + l2.Y + 1));

        }
        [TestMethod]
        public void PointToConsole_NoParent_NoChange()
        {
            var stubbedWindow = new StubbedWindow
            {
                PointToClientPoint = p => p,
                PointToConsolePoint = p => p
            };

            var l2 = new Point(23, 42);

            var sut2 = new Panel(stubbedWindow)
            {
                Location = l2
            };

            var clientPoint = new Point(123, 456);
            sut2.PointToConsole(clientPoint)
                .Should()
                .Be(clientPoint);

        }
    }
}
