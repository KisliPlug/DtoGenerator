using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis;

namespace DtoGenerator.Generator.Generators.Handlers.Interfaces;

public interface IDtoHandler<in T> where T : SyntaxNode
{
    public DtoGenerator.SourceData Generate(T syntax, DtoGeneratorAttribute attr);
}
