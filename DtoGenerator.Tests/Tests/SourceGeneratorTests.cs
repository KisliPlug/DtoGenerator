using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace DtoGenerator.Tests.Tests;

public class SourceGeneratorTests
{
    public SourceGeneratorTests()
    {

        _trees = Directory.GetFiles(@"Resources").Select(x => CSharpSyntaxTree.ParseText(File.ReadAllText(x))).ToArray();
    }

#region Private fields

    private readonly SyntaxTree[] _trees;

#endregion

    [Fact()]
    public void TestSouceGeneration_TEST()
    {
        //Arrange
        var dtoGenerator = Generator.Generators.DtoGenerator.Create(_trees);
        foreach (var dto in dtoGenerator.GenerateDtos())
        {
            Fixture.AddSource($"{dto.FileName}.g.cs", dto.Source);
        }
        //Act


        //Assert
        Assert.True(false);
    }
}
