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
using System.Threading.Tasks;
using ConControls.Logging;
using ConControlsTests.Examples;
using ConControlsTests.UnitTests;

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static async Task RunExampleAsync<T>() where T : Example, new()
        {
            var example = new T();
            Logger.Context = example.DebugContext;
            await example.RunAsync();
        }
        static async Task Main()
        {
            using var fileLogger = new FileLogger("concontrols.log");
            using var consoleLogger = new TestLogger(Console.WriteLine);

            Console.WriteLine("Starting test.");
            try
            {
                //await RunExampleAsync<ProgressBarExample>();
                await RunExampleAsync<TextBlockExample>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Test finished.");
        }
    }
}
