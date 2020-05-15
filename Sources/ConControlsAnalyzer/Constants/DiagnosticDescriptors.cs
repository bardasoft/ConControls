/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using Microsoft.CodeAnalysis;

namespace ConControls.Analyzer.Constants
{
    static class DiagnosticDescriptors
    {
        public static readonly DiagnosticDescriptor DeferDrawing = new DiagnosticDescriptor(
                id: DiagnosticIds.DeferDrawing,
                title: Resources.CC1001_Title,
                messageFormat: Resources.CC1001_MessageFormat,
                description: Resources.CC1001_Description,
                category: DiagnosticCategories.ConControls,
                defaultSeverity: DiagnosticSeverity.Info,
                isEnabledByDefault: true);
    }
}
