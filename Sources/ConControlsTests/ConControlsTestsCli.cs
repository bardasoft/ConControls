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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using ConControls.Logging;
using ConControlsTests.Examples;


namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static void RunTest()
        {
            ProgressBarExample.Run();
            Console.ReadLine();
        }

        static void Main()
        {
            using var logger = new FileLogger("concontrols.log");
            Logger.Context = DebugContext.None;

            try
            {
                RunTest();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            Console.WriteLine("Test finished.");
            Debug.WriteLine("Done.");
        }
    }
}
