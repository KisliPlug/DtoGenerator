using DtoGenerator.Generator.Generators.Handlers.Interfaces;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Generators.Handlers;

public class RecordDtoHandler : AbstractDtoHandler<RecordDeclarationSyntax>
{
    private bool _haveFullSyntax;

    public override DtoGenerator.SourceData GenerateLogic(RecordDeclarationSyntax syntax, DtoGeneratorAttribute attr)
    {
        if (!CollectAttributeAgrs(attr, out List<string> attrValues))
        {
            return Empty();
        }

        _haveFullSyntax = IsFullSyntaxRecord(syntax);
        var (ignoredNames, newName, namespaceName) = GetCreateionData(syntax, attrValues);
        var (membersData, propertyDescriptions, ignoredFields) = CollectRecordFields(syntax, ignoredNames);

        if (membersData is null || propertyDescriptions is null)
        {
            return Empty();
        }

        var ignoredPropsSyntax = ignoredFields
                               ?.Select(AddIgnoredPropDefaultValueCreator)
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList();
        var names = membersData.GetNodeNames().ToArray();

        return GenerateSyntax(namespaceName, newName, propertyDescriptions, names, ignoredPropsSyntax);
    }





    private bool IsFullSyntaxRecord(RecordDeclarationSyntax syntax)
    {
        return syntax.ParameterList == null;
    }

    protected override string GetCreateNewInstanceBracket(bool isOpen, string createEntityName)
    {
        if (_haveFullSyntax || createEntityName != _currentNodeName)
        {
            return isOpen ? "{" : "};";
        }

        return isOpen ? "(" : ");";
    }

    protected override string AssignConstructorSymbol(string createEntityName)
    {
        return _haveFullSyntax || createEntityName != _currentNodeName ? "="  :":" ;
    }

    protected override string GetDataType() => "record";

    protected override IEnumerable<string> AddProps(IEnumerable<string> fields, string name, string dtoName, bool set)
    {
        var retInditefer = set ? "dto" : "this";

        string setProp(string propName)
        {
            return $"{propName}=this.{propName}";
        }

        string getProp(string propName)
        {
            return $"{propName}=dto.{propName}";
        }

        var funcPrefix = set ? "Set" : "Get";
        Func<string, string> func = set ? setProp : getProp;
        var dataFields = fields
                        .Select(x => func(x))
                        .ToList()
                        .PipeAct(AddSeparator);
        var retType = set ? dtoName : name;
        yield return $"public {retType} {funcPrefix}Props({dtoName} dto)";
        yield return "{";
        yield return $"return {retInditefer} with";
        yield return "{";
        foreach (var field in dataFields)
        {
            yield return field;
        }

        yield return "};";
        yield return "}";
    }
}
