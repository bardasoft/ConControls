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
using ConControls.Controls;
using ConControls.Logging;
using ConControlsTests.UnitTests;

namespace ConControlsTests
{
    [ExcludeFromCodeCoverage]
    static class ConControlsTestsCli
    {
        static async Task Main()
        {
            using var consoleLogger = new TestLogger(Console.WriteLine);
            Logger.Context = DebugContext.Window;

            Console.WriteLine("Starting test.");
            try
            {
                using var window = new ConsoleWindow
                {
                    SwitchConsoleBuffersKey = KeyCombination.F11,
                    CloseWindowKey = KeyCombination.AltF4
                };
                await window.WaitForCloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Test finished.");
            Console.ReadLine();
        }
    }
}
