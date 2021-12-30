using Microsoft.CodeAnalysis;
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
        private const string PARTIAL = "partial";
        private const string GENERATE_ITEM_PROPERTY = "GenerateItemProperties";

        public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax cds &&
                cds.Modifiers
                    .Any(m => m.Text == PARTIAL) &&
                cds.BaseList is not null &&
                cds.BaseList.Types
                    .Any(t => t is SimpleBaseTypeSyntax sbts &&
                         sbts.Type is GenericNameSyntax gns &&
                         gns.Identifier.Text == ITEM_VIEW_MODEL_NAME) &&
                cds.AttributeLists
                    .Any(a => a.Attributes
                        .Any(aa => aa is AttributeSyntax asx &&
                            asx.Name is IdentifierNameSyntax ins &&
                            ins.Identifier.Text == GENERATE_ITEM_PROPERTY)))
            {
                CandidateClasses.Add(cds);
            }
        }
    }
}
