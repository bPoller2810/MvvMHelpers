using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvvMHelpers.Generators.ItemViewModel
{
    internal class ItemViewModelReceiver : ISyntaxReceiver
    {
        private const string ITEM_VIEW_MODEL_NAME = "BaseItemViewModel";
        private const string GENERATE_ITEM_PROPERTY = "GenerateItemProperties";

        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax
                {
                    BaseList.Types: { } baseListTypes,
                    Modifiers: { } modifiers,
                    AttributeLists: { } attributeLists
                }
                && baseListTypes.Any(t => t is SimpleBaseTypeSyntax //check the base type
                {
                    Type: GenericNameSyntax
                    {
                        Identifier.Text: ITEM_VIEW_MODEL_NAME
                    }
                })
                && modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)) //ensure the class is partial
                && attributeLists //check tif the class is decorated with our attribute
                    .SelectMany(a => a.Attributes)
                    .Any(aa => aa is AttributeSyntax
                    {
                        Name: IdentifierNameSyntax
                        {
                            Identifier.Text: GENERATE_ITEM_PROPERTY
                        }
                    }))
            {
                CandidateClasses.Add(syntaxNode as ClassDeclarationSyntax);
            }
        }

    }
}
