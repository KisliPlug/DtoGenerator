using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Orders.CodeGen.Generators;
using Orders.CodeGen.Recievers;

namespace Orders.CodeGen;

[Generator]
public class FillRangeGen : ISourceGenerator
{
#region Explicit interface implementation

    public void Initialize(GeneratorInitializationContext context)
    {
        // context.RegisterForSyntaxNotifications(() => new AttributeAggregate());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        "Go".Log();
        var dtoGenerator = DtoGenerator.Create(context.Compilation.SyntaxTrees);
        foreach (var dto in dtoGenerator.GenerateDtos())
        {
            context.AddSource($"{dto.FileName}.g.cs", dto.Source);
        }
    }

#endregion
}
