using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    public static partial class Generator
    {
        public static CheckedExpressionSyntax Checked(ExpressionSyntax expression)
        {
            return SyntaxFactory.CheckedExpression(SyntaxKind.CheckedExpression, expression);
        }

        public static FixedStatementSyntax Fixed(ExpressionSyntax expression)
        {
            return SyntaxFactory.FixedStatement(Variable("string", "a"), Block());
        }

        public static ExpressionSyntax ElementAccess(ExpressionSyntax expression, BracketedArgumentListSyntax args = null)
        {
            //	SyntaxFactory.
            return args == null ?
                SyntaxFactory.ElementAccessExpression(expression) :
                SyntaxFactory.ElementAccessExpression(expression, args);
        }

        public static StatementSyntax Statement(ExpressionSyntax expression)
        {
            return SyntaxFactory.ExpressionStatement(expression);
        }

        #region Assignment Expressions

        public static AssignmentExpressionSyntax Assignment(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Assignment(SyntaxKind.SimpleAssignmentExpression, left, right);
        }

        public static AssignmentExpressionSyntax AddAssignment(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Assignment(SyntaxKind.AddAssignmentExpression, left, right);
        }

        public static AssignmentExpressionSyntax SubtractAssignment(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Assignment(SyntaxKind.SubtractAssignmentExpression, left, right);
        }

        internal static AssignmentExpressionSyntax Assignment(SyntaxKind kind, ExpressionSyntax left, ExpressionSyntax right)
        {
            return SyntaxFactory.AssignmentExpression(kind, left, right);
        }

        #endregion

        #region Binary Expressions

        public static BinaryExpressionSyntax Add(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.AddExpression, left, right);
        }

        public static BinaryExpressionSyntax Subtract(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.SubtractExpression, left, right);
        }

        public static BinaryExpressionSyntax Multiply(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.MultiplyExpression, left, right);
        }

        public static BinaryExpressionSyntax Divide(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.DivideExpression, left, right);
        }

        public static BinaryExpressionSyntax Modulo(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.ModuloExpression, left, right);
        }

        public static BinaryExpressionSyntax LeftShift(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.RightShiftExpression, left, right);
        }

        public static BinaryExpressionSyntax RightShift(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.LeftShiftExpression, left, right);
        }

        public static BinaryExpressionSyntax LogicalOr(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.LogicalOrExpression, left, right);
        }

        public static BinaryExpressionSyntax LogicalAnd(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.LogicalAndExpression, left, right);
        }

        public static BinaryExpressionSyntax BitwiseOr(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.BitwiseOrExpression, left, right);
        }

        public static BinaryExpressionSyntax BitwiseAnd(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.BitwiseAndExpression, left, right);
        }

        public static BinaryExpressionSyntax ExclusiveOr(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.ExclusiveOrExpression, left, right);
        }

        public static BinaryExpressionSyntax As(string name, string type)
        {
            return As(SyntaxFactory.ParseExpression(name), SyntaxFactory.ParseExpression(type));
        }

        public static BinaryExpressionSyntax As(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.AsExpression, left, right);
        }

        public static BinaryExpressionSyntax Is(string name, string type)
        {
            return Is(SyntaxFactory.ParseExpression(name), SyntaxFactory.ParseExpression(type));
        }

        public static BinaryExpressionSyntax Is(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.IsExpression, left, right);
        }

        public static BinaryExpressionSyntax Equals(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.EqualsExpression, left, right);
        }

        public static BinaryExpressionSyntax NotEquals(ExpressionSyntax left, ExpressionSyntax right)
        {
            return Binary(SyntaxKind.NotEqualsExpression, left, right);
        }

        internal static BinaryExpressionSyntax Binary(SyntaxKind kind, ExpressionSyntax left, ExpressionSyntax right)
        {
            return SyntaxFactory.BinaryExpression(kind, left, right);
        }

        #endregion

        public static EqualsValueClauseSyntax EqualsValue(ExpressionSyntax expression)
        {
            return SyntaxFactory.EqualsValueClause(expression);
        }

        #region Unary Expressions

        public static PrefixUnaryExpressionSyntax BitwiseNot(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.BitwiseNotExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax LogicalNot(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.LogicalNotExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax PrefixPlus(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.UnaryPlusExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax PrefixMinus(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.UnaryMinusExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax PreIncrement(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.PreIncrementExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax PreDecrement(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.PreDecrementExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax AddressOf(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.AddressOfExpression, expression);
        }

        public static PrefixUnaryExpressionSyntax PointerIndirection(ExpressionSyntax expression)
        {
            return PrefixUnary(SyntaxKind.PointerIndirectionExpression, expression);
        }

        public static PostfixUnaryExpressionSyntax PostIncrement(ExpressionSyntax expression)
        {
            return PostfixUnary(SyntaxKind.PostIncrementExpression, expression);
        }

        public static PostfixUnaryExpressionSyntax PostDecrement(ExpressionSyntax expression)
        {
            return PostfixUnary(SyntaxKind.PostDecrementExpression, expression);
        }

        internal static PostfixUnaryExpressionSyntax PostfixUnary(SyntaxKind kind, ExpressionSyntax expression)
        {
            return SyntaxFactory.PostfixUnaryExpression(kind, expression);
        }

        internal static PrefixUnaryExpressionSyntax PrefixUnary(SyntaxKind kind, ExpressionSyntax expression)
        {
            return SyntaxFactory.PrefixUnaryExpression(kind, expression);
        }

        #endregion
    }
}