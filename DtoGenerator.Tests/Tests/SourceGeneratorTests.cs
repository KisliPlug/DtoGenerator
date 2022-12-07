using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Orders.CodeGen.Generators;

namespace CodeGen.Tests.Tests;

public class SourceGeneratorTests
{
    private readonly SyntaxTree[] _trees;

    public SourceGeneratorTests()
    {

        _trees = Directory.GetFiles(@"Resources").Select(x => CSharpSyntaxTree.ParseText(File.ReadAllText(x))).ToArray();
    }

    [Fact()]
    public void TestSouceGeneration_TEST()
    {
        //Arrange
        var dtoGenerator = DtoGenerator.Create(_trees);
        foreach (var dto in dtoGenerator.GenerateDtos())
        {
            Fixture.AddSource($"{dto.FileName}.g.cs", dto.Source);
        }
        //Act


        //Assert
        Assert.True(false);
    }
}
