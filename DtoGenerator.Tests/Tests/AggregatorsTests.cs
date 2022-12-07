using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using MoreLinq;
using Orders.CodeGen.Recievers;

namespace CodeGen.Tests.Tests;

public class AggregatorsTests
{
    private readonly SyntaxTree[] _trees;

    public AggregatorsTests()
    {
        _trees = Directory.GetFiles(@"Resources").Select(x => CSharpSyntaxTree.ParseText(File.ReadAllText(x))).ToArray();
    }

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
