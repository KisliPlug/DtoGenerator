using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq.Extensions;
using Orders.CodeGen.Generators.Handlers;
using Orders.CodeGen.Recievers;

namespace Orders.CodeGen.Generators;

public class DtoGenerator
{
    private readonly SyntaxTree[] _trees;
    private readonly List<ClassDeclarationSyntax> _generatorAttributes;

    private DtoGenerator(IEnumerable<SyntaxTree> trees, List<ClassDeclarationSyntax> generatorAttributes)
    {
        _trees = trees as SyntaxTree[] ?? trees.ToArray();
        _generatorAttributes = generatorAttributes;
    }

    public static DtoGenerator Create(IEnumerable<SyntaxTree> trees)
    {
        var attributeNameAggregate = new CorrectAttributeNameAggregate();
        var syntaxTrees = trees as SyntaxTree[] ?? trees.ToArray();
        foreach (SyntaxTree tree in syntaxTrees)
        {
            attributeNameAggregate.RecieveAllNode(tree);
        }

        return new DtoGenerator(syntaxTrees, attributeNameAggregate.CapturesClasses);
    }

    public IEnumerable<SourceData> GenerateDtos()
    {
        var aggregator = new AttributeAggregate(_generatorAttributes);
        _trees.ForEach(x => aggregator.RecieveAllNode(x));
        var classDtoGenerator = new ClassDtoHandler();
        var recordDtoGenerator = new RecordDtoHandler();
        foreach (var classCapture in aggregator.Captures)
        {
            yield return classCapture.Node switch
                         {
                             ClassDeclarationSyntax cl   => classDtoGenerator.Generate(cl, classCapture.Attr)
                           , RecordDeclarationSyntax rec => recordDtoGenerator.Generate(rec, classCapture.Attr)
                           , _                           => new SourceData("", "")
                         };
        }
    }

    public record SourceData(string FileName, string Source);
}
