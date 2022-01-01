using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace MvvMHelpers.Generators.ItemViewModel
{

    [Generator]
    public class ItemViewModelPropertyGenerator : ISourceGenerator
    {

        private const string RECORD_EQUALITY_CONTRACT = "EqualityContract";

        #region ISourceGenerator
        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not ItemViewModelReceiver receiver)
            {//should not happen, but to ensure the correct type and pattern cast it we do it anyway
                return;
            }

            foreach (var vmClass in receiver.CandidateClasses)
            {
                #region gather required information
                var vmSymbol = GetItemViewModelSymbol(vmClass, context.Compilation);
                var targetNamespace = vmSymbol.ContainingNamespace.ToDisplayString();
                var className = vmSymbol.Name;
                var itemIsRecord = IsItemTypeARecord(vmSymbol);
                var itemProperties = GetItemProperties(vmSymbol);
                var generatedFilename = BuildFilename(vmClass.SyntaxTree.FilePath);
                #endregion

                var sb = new StringBuilder();

                #region start namespace and class
                sb.AppendLine($"namespace {targetNamespace}");
                sb.AppendLine("{");
                sb.AppendLine($"\tpublic partial class {className}");
                sb.AppendLine("\t{");
                sb.AppendLine();
                #endregion

                #region add properties
                foreach (var prop in itemProperties)
                {
                    AddProperty(prop.Type.Name, prop.Name, prop.Type.NullableAnnotation == NullableAnnotation.Annotated, itemIsRecord, ref sb);
                    sb.AppendLine();
                }
                #endregion

                #region end class and namespace
                sb.AppendLine("\t}");
                sb.AppendLine("}");
                #endregion
                context.AddSource(generatedFilename, sb.ToString());
                
                System.Console.WriteLine($"Generated {itemProperties.Count()} ItemViewModel properties for {className}");
            }

        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                //Debugger.Launch(); // just for debugging
            }
#endif
            context.RegisterForSyntaxNotifications(() => new ItemViewModelReceiver());
        }
        #endregion

        #region helper
        private INamedTypeSymbol GetItemViewModelSymbol(ClassDeclarationSyntax cds, Compilation compilation)
        {
            return compilation.GetSemanticModel(cds.SyntaxTree).GetDeclaredSymbol(cds) as INamedTypeSymbol;
        }
        private IEnumerable<IPropertySymbol> GetItemProperties(INamedTypeSymbol classSymbol)
        {
            return classSymbol
                .BaseType
                .TypeArguments
                .First()
                .GetMembers()
                .Where(m => m is IPropertySymbol s && 
                    !s.IsReadOnly && 
                    s.DeclaredAccessibility == Accessibility.Public && //not readonly and public should be enough, but as nuget here happens interesting stuff...
                    s.Name != RECORD_EQUALITY_CONTRACT)
                .Cast<IPropertySymbol>();
        }
        private bool IsItemTypeARecord(INamedTypeSymbol classSymbol)
        {
            return classSymbol
                .BaseType
                .TypeArguments
                .First()
                .IsRecord;
        }
        private string BuildFilename(string originalPath)
        {
            var fileName = Path.GetFileNameWithoutExtension(originalPath);
            var fileExtension = Path.GetExtension(originalPath);
            return $"{fileName}.mvvm.{fileExtension}";
        }
        private void AddProperty(string type, string name, bool isNullable, bool isRecord, ref StringBuilder sb)
        {
            sb.AppendLine($"\t\tpublic {type}{(isNullable ? "?" : string.Empty)} {name}");
            sb.AppendLine("\t\t{");
            sb.AppendLine($"\t\t\tget => Item.{name};");
            sb.AppendLine("\t\t\tset");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine($"\t\t\t\tif (Item.{name} != value)");
            sb.AppendLine("\t\t\t\t{");
            sb.AppendLine($"\t\t\t\t\t{GetPropertyAssignString(name, isRecord)}");
            sb.AppendLine("\t\t\t\t\tOnPropertyChanged();");
            sb.AppendLine("\t\t\t\t}");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
        }
        private string GetPropertyAssignString(string name, bool isRecord)
        {
            return isRecord
                ? $"Item = Item with {{ {name} = value }};"
                : $"Item.{name} = value;";
        }
        #endregion

    }
}
