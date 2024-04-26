using System.Text;
using Galaxon.Core.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Galaxon.Maui.Generators;

[Generator]
public class MakeBindablePropertyGenerator : ISourceGenerator
{
    /// <inheritdoc />
    public void Initialize(GeneratorInitializationContext context) { }

    /// <inheritdoc />
    public void Execute(GeneratorExecutionContext context)
    {
        // Logic to find fields marked with [Bindable] and generate properties
        foreach (SyntaxTree syntaxTree in context.Compilation.SyntaxTrees)
        {
            SyntaxNode root = syntaxTree.GetRoot();
            IEnumerable<FieldDeclarationSyntax> fields = root.DescendantNodes().OfType<FieldDeclarationSyntax>();

            foreach (FieldDeclarationSyntax field in fields)
            {
                bool hasBindableAttribute = field.AttributeLists
                    .SelectMany(a => a.Attributes)
                    .Any(a => a.Name.ToString() == "MakeBindableProperty");

                if (hasBindableAttribute)
                {
                    string fieldName = field.Declaration.Variables.First().Identifier.ValueText;
                    string fieldType = field.Declaration.Type.ToString();
                    string propertyCode = GeneratePropertyCode(fieldName, fieldType);
                    context.AddSource($"{fieldName}_property.cs",
                        SourceText.From(propertyCode, Encoding.UTF8));
                }
            }
        }
    }

    private string PropertyNameFromFieldName(string fieldName)
    {
        if (char.IsUpper(fieldName[0]))
        {
            throw new ArgumentFormatException(nameof(fieldName),
                "Field names cannot begin with an upper-case letter.");
        }

        fieldName = fieldName.Trim('_');
        return fieldName[0].ToString().ToUpper() + fieldName[1..];
    }

    private string GeneratePropertyCode(string fieldName, string fieldType)
    {
        string propertyName = PropertyNameFromFieldName(fieldName);
        return $@"
private {fieldType} {fieldName};

public {fieldType} {propertyName}
{{
    get => {fieldName};
    set => SetProperty(ref {fieldName}, value);
}}";
    }
}
