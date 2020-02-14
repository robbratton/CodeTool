using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CommandLine
{
    public class SyntaxWrapper
    {
        private readonly SyntaxTree _tree;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        public SyntaxWrapper(string path)
        {
            var content = File.ReadAllText(path);

            _tree = CSharpSyntaxTree.ParseText(content);
        }

        #region Public Methods

        public IEnumerable<MethodDeclarationSyntax> GetMethodsFromClass(
            ClassDeclarationSyntax @class, 
            params Modifier[] modifiers) => GetMembersFromClass<MethodDeclarationSyntax>(@class, modifiers);

        public IEnumerable<ClassDeclarationSyntax> GetClassesFromTree(params Modifier[] modifiers) => GetMembersFromTree<ClassDeclarationSyntax>(modifiers);

        public IEnumerable<T> GetMembersFromClass<T>(ClassDeclarationSyntax @class, params Modifier[] modifiers)
            where T : MemberDeclarationSyntax
        {
            var members = @class.Members
                    .OfType<T>()
                ;
            if (modifiers != null && modifiers.Any())
            {
                members = members
                    .Where(member => 
                        member.Modifiers.Any(memberModifier => 
                            modifiers.Any(modifier => modifier.ToString().Equals( 
                                memberModifier.Text, StringComparison.CurrentCultureIgnoreCase))));
            }

            return members;
        }

        public IEnumerable<T> GetMembersFromTree<T>(params Modifier[] modifiers)
        where T : MemberDeclarationSyntax
        {
            var members = _tree.GetRoot().DescendantNodes()
                    .OfType<T>()
                ;
            if (modifiers != null && modifiers.Any())
            {
                members = members
                    .Where(member => 
                        member.Modifiers.Any(memberModifier => 
                            modifiers.Any(modifier => modifier.ToString().Equals( memberModifier.Text, StringComparison.CurrentCultureIgnoreCase))));
            }

            return members;
        }

        #endregion Public Methods

    }

    public enum Modifier
    {
        Public,
        Private,
        Protected,
        Internal,
        Abstract, 
        Override,
        New
    }
}
