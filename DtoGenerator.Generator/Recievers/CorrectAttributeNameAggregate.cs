using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Recievers;

public class CorrectAttributeNameAggregate : ISyntaxReceiver
{
#region Explicit interface implementation

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is not ClassDeclarationSyntax { BaseList: { } } attr)
        {
            return;
        }

        if (attr.BaseList.Types.Any(x => x.GetName().Equals(nameof(IPropertyConfig))))
        {
            CapturesClasses.Add(attr);
        }
    }

#endregion

    public List<ClassDeclarationSyntax> CapturesClasses { get; } = new();
}
