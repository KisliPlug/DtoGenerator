using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MoreLinq;

namespace DtoGenerator.Tests.Tests;

public class AggregatorsTests
{
    public AggregatorsTests()
    {
        _trees = Directory.GetFiles(@"Resources").Select(x => CSharpSyntaxTree.ParseText(File.ReadAllText(x))).ToArray();
    }

#region Private fields

    private readonly SyntaxTree[] _trees;

#endregion

    [Fact]
    public void CorrectAttributeNameAggregateCanFind1Attribute_Test()
    {
        //Arrange
        var attributeNameAggregate = new CorrectAttributeNameAggregate();
        //Act
        _trees.ForEach(x => attributeNameAggregate.RecieveAllNode(x));
        //Assert
        Assert.Single(attributeNameAggregate.CapturesClasses);
    }

    [Fact]
    public void AttributeAggregateCanFind1AllClassesAttributes_Test()
    {
        //Arrange
        var attributeNameAggregate = new CorrectAttributeNameAggregate();
        _trees.ForEach(x => attributeNameAggregate.RecieveAllNode(x));

        //Act
        var aggregator = new AttributeAggregate(attributeNameAggregate.CapturesClasses);
        _trees.ForEach(x => aggregator.RecieveAllNode(x));
        //Assert
        Assert.Equal(3, aggregator.Captures.Count);
    }
}
