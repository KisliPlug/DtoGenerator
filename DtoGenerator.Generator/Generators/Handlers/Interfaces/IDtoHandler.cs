using Microsoft.CodeAnalysis;
using Orders.CodeGen.Recievers;

namespace Orders.CodeGen.Generators.Handlers.Interfaces;

public interface IDtoHandler<in T> where T : SyntaxNode
{
    public DtoGenerator.SourceData Generate(T syntax, DtoGeneratorAttribute attr);
}
