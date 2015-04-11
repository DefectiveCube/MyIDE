using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Core
{
    /// <summary>
    /// This class provides code generation capabilities
    /// </summary>
    public static partial class Generator
    {
        public enum TypeAccessModifier
        {
            Unspecified,
            Internal,
            Public
        }

        public enum MemberAccessModifier
        {
            Unspecified,
            Private,
            Protected,
            Protected_Internal,
            Internal,
            Public
        }

        public static SyntaxNode Namespace(string name, IEnumerable<TypeDeclarationSyntax> types = null)
        {
            return SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name));
        }

        public static EventDeclarationSyntax Event(string name, TypeSyntax type)
        {
            return SyntaxFactory.EventDeclaration(type, name);
        }

        public static FieldDeclarationSyntax Field(string name, TypeSyntax type, SyntaxList<AttributeListSyntax> attributes = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return SyntaxFactory.FieldDeclaration(attributes, modifiers, Variable(type, name));                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the interface</param>
        /// <param name="access">The access level of the interface. Default: Unspecified</param>
        /// <returns></returns>
        public static InterfaceDeclarationSyntax Interface(string name, TypeAccessModifier access = TypeAccessModifier.Unspecified)
        {
            return SyntaxFactory.InterfaceDeclaration(name)
                .AddModifiers(SyntaxKind.PublicKeyword.ToToken());
        }

        public static AccessorDeclarationSyntax GetterAuto(MemberAccessModifier access = MemberAccessModifier.Unspecified)
        {
            throw new NotImplementedException();
            //return Accessor
        }

        internal static AccessorDeclarationSyntax Accessor(SyntaxKind kind, SyntaxToken keyword, BlockSyntax block = null)
        {
            return block != null ?
                SyntaxFactory.AccessorDeclaration(kind, block) :
                SyntaxFactory.AccessorDeclaration(kind, default(SyntaxList<AttributeListSyntax>), default(SyntaxTokenList), keyword, block, SyntaxFactory.Token(SyntaxKind.SemicolonToken));
        }

        public static PropertyDeclarationSyntax Property(
            string name,
            TypeSyntax type,
            TypeAccessModifier access = TypeAccessModifier.Unspecified,
            AccessorDeclarationSyntax getter = null,
            AccessorDeclarationSyntax setter = null
        )
        {
            var list = SyntaxFactory.AccessorList();

            if (getter != null)
            {
                list = list.AddAccessors(new[] { getter });
            }

            if (setter != null)
            {
                list = list.AddAccessors(new[] { setter });
            }

            return SyntaxFactory.PropertyDeclaration(type, name)
                    .WithAccessorList(list);
        }

        public static NameSyntax Name(string name)
        {
            return SyntaxFactory.ParseName(name);
        }

        public static TypeSyntax TypeName(string type)
        {
            return SyntaxFactory.ParseTypeName(type);
        }

        public static ClassDeclarationSyntax Class(
            string name,
            TypeAccessModifier access = TypeAccessModifier.Unspecified,
            string baseClass = "System.Object",
            TypeSyntax parent = null,
            bool isAbstract = false,
            bool isSealed = false,
            bool isStatic = false,
            IEnumerable<MemberDeclarationSyntax> members = null,
            IEnumerable<string> interfaces = null,
            IEnumerable<string> attributes = null)
        {
            var clsDec = SyntaxFactory.ClassDeclaration(name)
                .WithOpenBraceToken(SyntaxKind.OpenBraceToken)
                .WithMembers(SyntaxFactory.List<MemberDeclarationSyntax>(members))
                .WithCloseBraceToken(SyntaxKind.CloseBraceToken);

            // Determine the access modifier
            switch (access)
            {
                case TypeAccessModifier.Public:
                    clsDec = clsDec.WithKeyword(SyntaxKind.PublicKeyword);
                    break;
                case TypeAccessModifier.Internal:
                    clsDec = clsDec.WithKeyword(SyntaxKind.InternalKeyword);
                    break;
                default:
                    // Not specified
                    break;
            }

            // Set modifier keywords
            clsDec = isAbstract ? clsDec.WithKeyword(SyntaxKind.AbstractKeyword) : clsDec;
            clsDec = isSealed ? clsDec.WithKeyword(SyntaxKind.SealedKeyword) : clsDec;
            clsDec = isStatic ? clsDec.WithKeyword(SyntaxKind.StaticKeyword) : clsDec;

            var baseList = SyntaxFactory.BaseList();

            // Add base type
            if (parent != null)
            {
                baseList = baseList.AddTypes(new[] { SyntaxFactory.SimpleBaseType(parent) });
            }

            // Implemented Interfaces
            if (interfaces != null)
            {
                var _interfaces = interfaces.Select(s => SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(s)));

                baseList = baseList.AddTypes(_interfaces.ToArray());
            }

            // Add inherited type and implemented interfaces
            clsDec = clsDec.WithBaseList(baseList);

            // TODO: add attributes
            // TODO: add generics

            return clsDec;
        }

        /// <summary>
        /// Creates an InterfaceDeclaration node with a specified name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static InterfaceDeclarationSyntax Interface(string name)
        {
            return SyntaxFactory.InterfaceDeclaration(name)
                .WithModifiers(
                    SyntaxFactory.TokenList(new SyntaxToken[] { SyntaxKind.PublicKeyword.ToToken() }));
        }

        public static SyntaxNode Enum(string name, BaseListSyntax baseClass = null)
        {
            return SyntaxFactory.EnumDeclaration(
                SyntaxFactory.List<AttributeListSyntax>(),
                SyntaxFactory.TokenList(),
                SyntaxFactory.ParseToken(name),
                baseClass,
                SyntaxFactory.SeparatedList<EnumMemberDeclarationSyntax>());
        }

        public static SyntaxNode Struct(string name)
        {
            return SyntaxFactory.StructDeclaration(name);
        }

        public static DelegateDeclarationSyntax Delegate(string name, TypeSyntax type = null)
        {
            type = type ?? SyntaxFactory.PredefinedType(SyntaxKind.VoidKeyword.ToToken());

            return SyntaxFactory.DelegateDeclaration(type, name);
        }

        public static ConstructorDeclarationSyntax Constructor(string name, MemberAccessModifier access = MemberAccessModifier.Unspecified, bool isStatic = false)
        {
            var dec = SyntaxFactory.ConstructorDeclaration(name)
                .WithBody(Block());

            switch (access)
            {
                case MemberAccessModifier.Private:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.PrivateKeyword.ToToken()));
                    break;
                case MemberAccessModifier.Protected:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.ProtectedKeyword.ToToken()));
                    break;
                case MemberAccessModifier.Public:
                    dec = dec.WithModifiers(SyntaxFactory.TokenList(SyntaxKind.PublicKeyword.ToToken()));
                    break;
                default:
                    break;
            }

            return dec;
        }

        public static DestructorDeclarationSyntax Destructor(string name, BlockSyntax body = null)
        {
            return body == null ?
                SyntaxFactory.DestructorDeclaration(name).WithBody(Block()) :
                SyntaxFactory.DestructorDeclaration(name).WithBody(body);
        }

        public static MethodDeclarationSyntax Method(
            string name,
            TypeSyntax returnType = null,
            MemberAccessModifier access = MemberAccessModifier.Unspecified,
            bool isAbstract = false,
            bool isExtension = false,
            bool isOverrides = false,
            bool isStatic = false,
            bool isVirtual = false,
            ParameterListSyntax parameters = null,
            BlockSyntax body = null
        )
        {
            // If returnType is null, set it to "void" type token
            returnType = returnType ?? SyntaxKind.VoidKeyword.ToTypeSyntax();

            // Instantiate method declaration
            var dec = SyntaxFactory.MethodDeclaration(returnType, name);

            // If method body is not passed, use an empty block
            dec = body != null ? dec.WithBody(body) : dec.WithBody(Block());

            if (isAbstract)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.AbstractKeyword.ToToken() });
            }

            if (isOverrides)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.OverrideKeyword.ToToken() });
            }

            if (isStatic || isExtension)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.StaticKeyword.ToToken() });
            }

            if (isVirtual)
            {
                dec = dec.AddModifiers(new[] { SyntaxKind.VirtualKeyword.ToToken() });
            }

            if (isExtension)
            {
                // TODO: ensure the first parameter has "this"                
            }

            dec = parameters != null ? dec.WithParameterList(parameters) : dec;

            return dec;
        }

        /// <summary>
        /// Construct a statement block with given statements
        /// </summary>
        public static BlockSyntax Block(SyntaxList<StatementSyntax> statements = default(SyntaxList<StatementSyntax>))
        {
            return SyntaxFactory.Block(
                SyntaxKind.OpenBraceToken.ToToken(),
                statements,
                SyntaxKind.CloseBraceToken.ToToken()
            );
        }

        #region Parameters

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="type">The type of the parameter</param>
        /// <param name="attributeLists">Custom attributes?</param>
        /// <param name="modifiers">ref/out modifiers?</param>
        public static ParameterSyntax Parameter(string name, string type, SyntaxList<AttributeListSyntax> attributeLists = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return Parameter(name, SyntaxFactory.ParseTypeName(type), attributeLists, modifiers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="attributeLists"></param>
        /// <param name="modifiers"></param>
        public static ParameterSyntax Parameter(string name, TypeSyntax type, SyntaxList<AttributeListSyntax> attributeLists = default(SyntaxList<AttributeListSyntax>), SyntaxTokenList modifiers = default(SyntaxTokenList))
        {
            return SyntaxFactory.Parameter(
                attributeLists,
                modifiers,
                type,
                SyntaxFactory.Identifier(name),
                null
            );
        }

        public static ParameterListSyntax Parameters()
        {
            return Parameters(new ParameterSyntax[] { });
        }

        public static ParameterListSyntax Parameters(ParameterSyntax p1)
        {
            return Parameters(new[] { p1 });
        }

        public static ParameterListSyntax Parameters(ParameterSyntax p1, ParameterSyntax p2)
        {
            return Parameters(new[] { p1, p2 });
        }

        public static ParameterListSyntax Parameters(params ParameterSyntax[] parameters)
        {
            return SyntaxFactory.ParameterList().AddParameters(parameters);
        }

        #endregion

#region Arrays

        /// <summary>
        /// Creates an array of specified type
        /// </summary>
        /// <param name="type">The type of the array</param>
        /// <returns></returns>
        public static ArrayTypeSyntax Array(string type)
        {
            return Array(SyntaxFactory.ParseTypeName(type))
                .WithRankSpecifiers(
                SyntaxFactory.List<ArrayRankSpecifierSyntax>().Add(
                    SyntaxFactory.ArrayRankSpecifier()
                )
            );
        }

        public static ArrayTypeSyntax Array(TypeSyntax type)
        {
            return SyntaxFactory.ArrayType(type);
        }

        public static ArrayCreationExpressionSyntax Array(string type, params ArrayRankSpecifierSyntax[] ranks)
        {
            return Array(SyntaxFactory.ParseTypeName(type), ranks);
        }

        public static ArrayCreationExpressionSyntax Array(TypeSyntax type, params ArrayRankSpecifierSyntax[] ranks)
        {
            return SyntaxFactory.ArrayCreationExpression(Array("string"));
        }

#endregion

        public static IfStatementSyntax If()
        {
            return SyntaxFactory.IfStatement(
                SyntaxFactory.ParseExpression("true"),
                SyntaxFactory.EmptyStatement());
        }

        public static ReturnStatementSyntax Return(ExpressionSyntax expression = null)
        {
            return SyntaxFactory.ReturnStatement()
                .WithExpression(expression);


            //.with (SyntaxFactory.Token(SyntaxKind.ReturnKeyword),expression:,;
        }

        public static WhileStatementSyntax While(ExpressionSyntax condition, StatementSyntax statement)
        {
            return SyntaxFactory.WhileStatement(condition, statement);
        }

        public static ForStatementSyntax For(StatementSyntax statement)
        {
            return SyntaxFactory.ForStatement(statement);
        }

        public static AssignmentExpressionSyntax Equals()
        {
            throw new NotImplementedException();
            //return SyntaxFactory.AssignmentExpression(SyntaxKind left, ExpressionSyntax left right);
        }

        /*public static ForEachStatementSyntax ForEach() {
			return SyntaxFactory.ForEachStatement(Type,identifier:,ExpressionSyntax:,StatementSyntax);
		}*/

        public static EmptyStatementSyntax Statement()
        {
            return SyntaxFactory.EmptyStatement();
        }

        public static UsingStatementSyntax Using(VariableDeclarationSyntax declaration = default(VariableDeclarationSyntax), ExpressionSyntax expression = null, StatementSyntax statement = default(StatementSyntax))
        {
            return SyntaxFactory.UsingStatement(
                SyntaxKind.UsingKeyword.ToToken(),
                SyntaxKind.OpenParenToken.ToToken(),
                declaration,
                expression,
                SyntaxKind.CloseParenToken.ToToken(),
                statement);
        }

        public static VariableDeclaratorSyntax Variable(string name)
        {
            return SyntaxFactory.VariableDeclarator(name);
        }

        public static VariableDeclarationSyntax Variable(string type, string name)
        {
            return Variable(SyntaxFactory.ParseTypeName(type), name);
        }

        public static VariableDeclarationSyntax Variable(TypeSyntax type, string value)
        {
            return SyntaxFactory.VariableDeclaration(type, SyntaxFactory.SeparatedList<VariableDeclaratorSyntax>()
                .Add(Variable(value)));
        }

        /*public static VariableDeclarationSyntax Variable(KeyValuePair<string,TypeSyntax> variables)
        {
            return SyntaxFactory.VariableDeclaration(t)
        }*/

        public static ObjectCreationExpressionSyntax ObjectCreation(TypeSyntax type)
        {
            return SyntaxFactory.ObjectCreationExpression(type);
        }

        public static InitializerExpressionSyntax Initializer(SeparatedSyntaxList<ExpressionSyntax> expressions = default(SeparatedSyntaxList<ExpressionSyntax>))
        {
            return SyntaxFactory.InitializerExpression(SyntaxKind.ObjectInitializerExpression, expressions);
        }

        public static InvocationExpressionSyntax Invoke(ExpressionSyntax expression = default(ExpressionSyntax), ArgumentListSyntax arguments = default(ArgumentListSyntax))
        {
            return SyntaxFactory.InvocationExpression(expression, SyntaxFactory.ArgumentList()
                .AddArguments(new ArgumentSyntax[] { SyntaxFactory.Argument(Generator.Literal("Hello")) }));
        }

        /*public static RefValueExpressionSyntax ObjectRef(){
			return SyntaxFactory.RefValueExpression (SyntaxFactory.ParseExpression ("Console"), TypeName ("W"));
		}*/

        public static ExpressionSyntax ParseExpression(string expression)
        {
            return SyntaxFactory.ParseExpression(expression);
        }

        public static MemberAccessExpressionSyntax[] Members(params MemberAccessExpressionSyntax[] members)
        {
            return members;
        }

        public static MemberAccessExpressionSyntax Member(string name, ExpressionSyntax expression, SyntaxKind kind = SyntaxKind.SimpleMemberAccessExpression)
        {
            return SyntaxFactory.MemberAccessExpression(
                kind,
                expression,
                SyntaxFactory.IdentifierName(name));
        }

        /*public static MemberBindingExpressionSyntax Member(string name){
			return SyntaxFactory.MemberBindingExpression (SyntaxFactory.IdentifierName (name));
		}*/

        /*public static NameOfExpressionSyntax Name()
        {
            throw new NotImplementedException();
            // SyntaxFactory.NameOfExpression("WriteLine", SyntaxFactory.ParseExpression("\"test\""));
        }*/

        public static TypeOfExpressionSyntax TypeOf(TypeSyntax type, SyntaxToken keyword = default(SyntaxToken))
        {
            return SyntaxFactory.TypeOfExpression(
                keyword,
                SyntaxKind.OpenParenToken.ToToken(),
                type,
                SyntaxKind.CloseBraceToken.ToToken()
            );
        }
    }
}
