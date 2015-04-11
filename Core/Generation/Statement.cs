using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    public static partial class Generator
    {
        public static ContinueStatementSyntax Continue()
        {
            return SyntaxFactory.ContinueStatement();
        }

        public static ThrowStatementSyntax Throw(ExpressionSyntax expression = null)
        {
            return SyntaxFactory.ThrowStatement(expression);
        }

        public static BreakStatementSyntax Break()
        {
            return SyntaxFactory.BreakStatement();
        }

        public static TryStatementSyntax Try(SyntaxList<CatchClauseSyntax> catches = default(SyntaxList<CatchClauseSyntax>))
        {
            return SyntaxFactory.TryStatement(catches);
        }

        public static CatchClauseSyntax Catch()
        {
            return SyntaxFactory.CatchClause();
        }

        public static FinallyClauseSyntax Finally(BlockSyntax block = null)
        {
            return SyntaxFactory.FinallyClause(block);
        }

        public static YieldStatementSyntax YieldBreak()
        {
            return SyntaxFactory.YieldStatement(SyntaxKind.YieldBreakStatement);
        }

        public static YieldStatementSyntax YieldReturn(ExpressionSyntax expression)
        {
            return SyntaxFactory.YieldStatement(SyntaxKind.YieldReturnStatement, expression);
        }
    }
}