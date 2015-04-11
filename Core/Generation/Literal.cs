using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    public static partial class Generator
    {
        public static LiteralExpressionSyntax Literal(string value)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value));
        }

        public static LiteralExpressionSyntax Literal(int value)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(value));
        }

        public static LiteralExpressionSyntax Literal(char value)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.CharacterLiteralExpression, SyntaxFactory.Literal(value));
        }

        public static LiteralExpressionSyntax True()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression);
        }

        public static LiteralExpressionSyntax False()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.FalseLiteralExpression);
        }

        public static LiteralExpressionSyntax Null()
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
        }
    }
}