using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    public static partial class Generator
    {
        public static BracketedArgumentListSyntax ToList(this ArgumentSyntax[] args)
        {
            return SyntaxFactory.BracketedArgumentList().AddArguments(args);
        }

        public static BracketedParameterListSyntax ToList(this ParameterSyntax[] args)
        {
            return SyntaxFactory.BracketedParameterList().AddParameters(args);
        }

        internal static SyntaxToken ToToken(this SyntaxKind kind)
        {
            return SyntaxFactory.Token(kind);
        }

        internal static TypeSyntax ToTypeSyntax(this SyntaxKind kind)
        {
            return SyntaxFactory.PredefinedType(SyntaxFactory.Token(kind));
        }

        internal static ClassDeclarationSyntax WithBaseList(this ClassDeclarationSyntax node, string type)
        {
            return WithBaseList(node, SyntaxFactory.ParseTypeName(type));
        }

        internal static ClassDeclarationSyntax WithBaseList(this ClassDeclarationSyntax node, TypeSyntax type)
        {
            return WithBaseList(node, new[] { type });
        }

        internal static ClassDeclarationSyntax WithBaseList(this ClassDeclarationSyntax node, params TypeSyntax[] types)
        {
            List<BaseTypeSyntax> baseTypes = new List<BaseTypeSyntax>();

            foreach (var t in types)
            {
                baseTypes.Add(SyntaxFactory.SimpleBaseType(t));
            }

            return node.WithBaseList(SyntaxFactory.BaseList().AddTypes(baseTypes.ToArray()));
        }

        public static ClassDeclarationSyntax WithPublicKeyword(this ClassDeclarationSyntax node)
        {
            return node.AddModifiers(SyntaxKind.PublicKeyword.ToToken());
        }

        internal static ClassDeclarationSyntax WithCloseBraceToken(this ClassDeclarationSyntax node, SyntaxKind kind)
        {
            return node.WithCloseBraceToken(SyntaxFactory.Token(kind));
        }

        internal static ClassDeclarationSyntax WithOpenBraceToken(this ClassDeclarationSyntax node, SyntaxKind kind)
        {
            return node.WithOpenBraceToken(SyntaxFactory.Token(kind));
        }

        internal static ClassDeclarationSyntax WithKeyword(this ClassDeclarationSyntax node, SyntaxKind kind)
        {
            return node.WithKeyword(SyntaxFactory.Token(kind));
        }
    }
}