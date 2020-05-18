/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

#nullable enable
#pragma warning disable IDE0051

// ReSharper disable UnusedMember.Local

using System;
using System.Diagnostics.CodeAnalysis;
using ConControls.Logging;
using ConControls.WindowsApi;
using ConControlsTests.Examples;

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static void RunTest()
        {
            ProgressBarExample.Run();
        }

        static void Main()
        {
            using var logger = new FileLogger("concontrols.log");
            Logger.Context = DebugContext.None;

            Console.WriteLine("Starting test.");
            var api = new NativeCalls();
            Console.WriteLine(api.GetConsoleMode(api.GetOutputHandle()));
            try
            {
                RunTest();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Test finished.");
            Console.WriteLine(api.GetConsoleMode(api.GetOutputHandle()));
        }
    }
}
