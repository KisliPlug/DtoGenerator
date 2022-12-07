using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Orders.CodeGen.Infra;

namespace Orders.CodeGen.Recievers;

public class CorrectAttributeNameAggregate : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> CapturesClasses { get; } = new();

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
}
