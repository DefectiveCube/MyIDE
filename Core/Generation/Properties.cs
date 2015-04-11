using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    public static partial class Generator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax Getter(MemberAccessModifier access = MemberAccessModifier.Unspecified, BlockSyntax block = null)
        {
            return Accessor(SyntaxKind.GetAccessorDeclaration, SyntaxFactory.Token(SyntaxKind.GetKeyword), block);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        public static AccessorDeclarationSyntax Setter(MemberAccessModifier access = MemberAccessModifier.Unspecified, BlockSyntax block = null)
        {
            return Accessor(SyntaxKind.SetAccessorDeclaration, SyntaxFactory.Token(SyntaxKind.SetKeyword), block);
        }
    }
}
