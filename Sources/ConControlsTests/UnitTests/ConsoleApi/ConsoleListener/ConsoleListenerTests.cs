/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable

using System.Diagnostics.CodeAnalysis;
using ConControls.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests.ConsoleApi.ConsoleListener
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public partial class ConsoleListenerTests
    {
        [TestInitialize]
        public void TestInitialize() => Logger.Context = DebugContext.ConsoleListener;
        [TestCleanup]
        public void TestCleanup() => Logger.Context = DebugContext.None;

    }
}
