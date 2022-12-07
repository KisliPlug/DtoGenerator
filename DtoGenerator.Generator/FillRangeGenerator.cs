using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis;

namespace DtoGenerator.Generator;

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
        var dtoGenerator = Generators.DtoGenerator.Create(context.Compilation.SyntaxTrees);
        foreach (var dto in dtoGenerator.GenerateDtos())
        {
            context.AddSource($"{dto.FileName}.g.cs", dto.Source);
        }
    }

#endregion
}
