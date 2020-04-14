using System.Diagnostics.CodeAnalysis;
using ConControls.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConControlsTests.UnitTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AssemblySettings
    {
        [AssemblyInitialize]
        [SuppressMessage("Style", "IDE0060", Justification = "Parameter required by test framework.")]
        public static void TestInitialize(TestContext testContext)
        {
            Logger.Context = DebugContext.All;
        }
    }
}
