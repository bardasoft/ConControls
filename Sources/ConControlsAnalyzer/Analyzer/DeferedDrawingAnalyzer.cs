/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

using System;
using System.Collections.Immutable;
using System.Linq;
using ConControls.Analyzer.Constants;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ConControls.Analyzer.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class DeferedDrawingAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(DiagnosticDescriptors.DeferDrawing);

        public override void Initialize(AnalysisContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(AnalyseSyntaxNode, SyntaxKind.SimpleAssignmentExpression);
        }
        private static void AnalyseSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            if (!(context.Node is AssignmentExpressionSyntax assignment)) return;
            var symbol = context.SemanticModel.GetSymbolInfo(assignment.Left);
            var typeInfo = context.SemanticModel.GetTypeInfo(assignment.Left);
            Console.WriteLine(typeInfo);
            //var namedTypeSymbol = context.Compilation.;
            //if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            //{
            //    var diagnostic = Diagnostic.Create(DiagnosticDescriptors.DeferDrawing, 
            //                                       namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
            //    context.ReportDiagnostic(diagnostic);
            //}
        }
    }
}
