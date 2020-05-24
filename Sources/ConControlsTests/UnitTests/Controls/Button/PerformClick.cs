/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToDisposedClosure

#nullable enable

namespace ConControlsTests.UnitTests.Controls.Button
{
    public partial class ButtonTests
    {
        [TestMethod]
        public void PerformClick_Disabled_Nothing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Parent = stubbedWindow,
                Enabled = false
            };
            bool clicked = false;
            sut.Click += (sender, e) => clicked = true;
            sut.PerformClick();
            clicked.Should().BeFalse();
        }
        [TestMethod]
        public void PerformClick_Invisible_Nothing()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Parent = stubbedWindow,
                Visible = false
            };
            bool clicked = false;
            sut.Click += (sender, e) => clicked = true;
            sut.PerformClick();
            clicked.Should().BeFalse();
        }
        [TestMethod]
        public void PerformClick_EventRaised()
        {
            using var stubbedWindow = new StubbedWindow();
            using var sut = new ConControls.Controls.Button(stubbedWindow)
            {
                Parent = stubbedWindow
            };
            bool clicked = false;
            sut.Click += OnClick;
            sut.PerformClick();
            clicked.Should().BeTrue();
            clicked = false;
            sut.Click -= OnClick;
            sut.PerformClick();
            clicked = false;

            void OnClick(object sender, EventArgs e)
            {
                sender.Should().Be(sut);
                clicked = true;
            }
        }
    }
}
