using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Recievers;

public class AttributeAggregate : ISyntaxReceiver
{
    public AttributeAggregate(List<ClassDeclarationSyntax> capturesClasses)
    {
        _attributeNames = capturesClasses.SelectMany(GetAtrName);
    }

#region Private fields

    private readonly IEnumerable<string> _attributeNames;

#endregion

#region Explicit interface implementation

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not AttributeSyntax attr || !_attributeNames.Contains(attr.Name.ToString()))
        {
            return;
        }

        if (syntaxNode.GetParent<ClassDeclarationSyntax>() is { } classDecl)
        {
            Captures.Add(new Capture(classDecl, new DtoGeneratorAttribute(attr)));
        }
        else if (syntaxNode.GetParent<RecordDeclarationSyntax>() is { } rec)
        {
            Captures.Add(new Capture(rec, new DtoGeneratorAttribute(attr)));
        }
    }

#endregion

    public List<Capture> Captures { get; } = new();

    private IEnumerable<string> GetAtrName(ClassDeclarationSyntax decl)
    {
        var name = decl.Identifier.Text;
        yield return name;
        if (name.EndsWith("Attribute"))
        {
            yield return name.Replace("Attribute", "");
        }
    }

    public record Capture(SyntaxNode Node, DtoGeneratorAttribute Attr)
    {
        public SyntaxNode Node { get; } = Node;
        public DtoGeneratorAttribute Attr { get; } = Attr;
    }
}

public class DtoGeneratorAttribute
{
    public DtoGeneratorAttribute(AttributeSyntax attr)
    {
        Arguments = attr.ArgumentList!.Arguments;
    }

#region Private fields

#endregion

    public SeparatedSyntaxList<AttributeArgumentSyntax> Arguments { get; }
}
