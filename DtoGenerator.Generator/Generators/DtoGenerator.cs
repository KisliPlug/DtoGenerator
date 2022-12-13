using DtoGenerator.Generator.Generators.Handlers;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace DtoGenerator.Generator.Generators;

public class DtoGenerator
{
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

#region Private fields

    private readonly List<ClassDeclarationSyntax> _generatorAttributes;
    private readonly SyntaxTree[] _trees;

#endregion

    public IEnumerable<SourceData> GenerateDtos()
    {
        var aggregator = new AttributeAggregate(_generatorAttributes);
        foreach (var tree in _trees)
        {
            aggregator.RecieveAllNode(tree);
        }

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

    public record SourceData(string FileName, string Source)
    {
        public string FileName { get; } = FileName;
        public string Source { get; } = Source;
    }
}
