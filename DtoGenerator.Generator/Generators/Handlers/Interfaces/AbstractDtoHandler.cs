using System.Collections;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Generators.Handlers.Interfaces;

public abstract class AbstractDtoHandler<T> : IDtoHandler<T> where T : SyntaxNode
{
#region Explicit interface implementation

    public DtoGenerator.SourceData Generate(T syntax, DtoGeneratorAttribute attr)
    {
        _currentNodeName = syntax.GetName();
        return GenerateLogic(syntax, attr);
    }

#endregion

    protected string _currentNodeName = "";
    public abstract DtoGenerator.SourceData GenerateLogic(T syntax, DtoGeneratorAttribute attr);

    protected DtoGenerator.SourceData Empty()
    {
        return new DtoGenerator.SourceData("", "");
    }

    protected IEnumerable<string> GenerateExtensions(string newName, string hostClassName)
    {
        var methodSyntaxes = new List<(string decl, string body)>
                             {
                                 ($"public static {newName} As{newName}(this {hostClassName} entity)", $"return ({newName})entity;")
                               , ($"public static {hostClassName} As{hostClassName}(this {newName} dto)", $"return ({hostClassName})dto;")
                               , ($"public static IEnumerable<{newName}> As{newName}(this IEnumerable<{hostClassName}> entity)"
                                , $"return  entity.Select(x=>x.As{newName}());")
                               , ($"public static IEnumerable<{hostClassName}> As{hostClassName}(this IEnumerable<{newName}> dtos)"
                                , $"return dtos.Select(x=>x.As{hostClassName}());")
                             };
        yield return $"public static class {newName}Extensions";
        yield return "{";
        foreach (var methodDescr in methodSyntaxes)
        {
            foreach (var line in GenerateMethod(methodDescr.decl, methodDescr.body))
            {
                yield return line;
            }
        }

        yield return "}";
    }

    private IEnumerable<string> GenerateMethod(string declaration, string body)
    {
        yield return declaration;
        yield return "{";
        yield return body;
        yield return "}";
    }

    protected string GetNamespaceName(SyntaxNode classDeclarationSyntax)
    {
        var retVal = "Default";
        if (classDeclarationSyntax.GetParent<NamespaceDeclarationSyntax>() is { } sp)
        {
            retVal = sp.Name.ToString();
        }
        else if (classDeclarationSyntax.GetParent<FileScopedNamespaceDeclarationSyntax>() is { } scp)
        {
            retVal = scp.Name.ToString();
        }

        return retVal;
    }

    protected IEnumerable<string> AddOperators(IEnumerable<string> fields, IEnumerable<string> ignored, string fromStr, string toStr)
    {
        var dataFields = fields
                        .Select(x => $"{x}{AssignConstructorSymbol(toStr)}b.{x}")
                        .ToList();
        foreach (var s in ignored)
        {
            dataFields.Add(s);
        }

        AddSeparator(dataFields);
        yield return $"public static explicit operator {toStr}({fromStr} b)";
        yield return "{";
        yield return $"return new {toStr}";
        yield return GetCreateNewInstanceBracket(true, toStr);
        foreach (var dd in dataFields)
        {
            yield return dd;
        }

        yield return GetCreateNewInstanceBracket(false, toStr);
        yield return "}";
    }

    protected abstract string GetCreateNewInstanceBracket(bool isOpen, string createEntityName);

    protected abstract string AssignConstructorSymbol(string createEntityName);

    protected abstract string GetDataType();

    protected void AddSeparator(List<string> dataFields)
    {
        for (int i = 0; i < dataFields.Count - 1; i++)
        {
            dataFields[i] += ",";
        }
    }

    protected string AddIgnoredPropDefaultValueCreator(SyntaxNode s)
    {
        var propType = "";
        var name = "";
        if (s is PropertyDeclarationSyntax prop)
        {
            propType = prop.Type.ToFullString();
            name = prop.Identifier.Text;
        }
        else if (s is ParameterSyntax parameter)
        {
            propType = parameter.Type?.ToString();
            name = parameter.Identifier.Text;
        }

        string getEnumerableCreation(string en)
        {
            return en.Replace($"{nameof(IEnumerable)}", "List");
        }

        var add = propType.Trim() switch
                  {
                      nameof(Guid)                                => $"{nameof(Guid)}.{nameof(Guid.NewGuid)}()"
                    , nameof(DateTimeOffset)                      => $"{nameof(DateTimeOffset)}.{nameof(DateTimeOffset.Now)}"
                    , nameof(DateTime)                            => $"{nameof(DateTime)}.{nameof(DateTime.Now)}"
                    , var str when str.StartsWith("List<")        => $"new()"
                    , var str when str.StartsWith("Dictionary<")  => $"new()"
                    , var str when str.StartsWith("IEnumerable<") => $"new {getEnumerableCreation(str)}()"
                    , _                                           => ""
                  };
        if (string.IsNullOrEmpty(add) || string.IsNullOrEmpty(name))
        {
            return "";
        }

        return $"{name}{AssignConstructorSymbol(_currentNodeName)}{add}";
    }

    protected abstract IEnumerable<string> AddProps(IEnumerable<string> fields, string name, string dtoName, bool set);








    protected bool CollectAttributeAgrs(DtoGeneratorAttribute attr, out List<string> attrValues)
    {
        attrValues = attr.Arguments.Select(x => x.Expression.ToString())
                         .Select(x => x.Replace("nameof(", "").Replace(")", "").Replace($"\"", ""))
                         .ToList();
        if (attrValues.Count < 1)
        {
            return false;
        }

        return true;
    }

    protected (List<string> ignorProps, string newName, string namespaceName) GetCreateionData(SyntaxNode syntax, List<string> attrValues)
    {
        return (attrValues.Skip(1).ToList(), $"{attrValues.First()}{syntax.GetName()}Dto", GetNamespaceName(syntax));
    }



    protected ( List<SyntaxNode>? nodes, List<string>? props, List<SyntaxNode>? ignored) CollectRecordFields(RecordDeclarationSyntax rce
                                                                                                           , List<string> ignorProps)
    {
        List<string> dd = new();
        List<SyntaxNode> membersData = new();
        List<SyntaxNode> ignored = new();
        if (rce.Members is { Count: > 0 } members)
        {
            ignored = members
                     .Where(x => ignorProps.Any(y => x.GetName().Equals(y)))
                     .ToList<SyntaxNode>();
            membersData = members
                         .Where(x => !ignorProps.Any(y => x.GetName().Equals(y)))
                         .ToList<SyntaxNode>();
            dd = membersData.Select(x => x.ToString())
                            .ToList();
            if (dd.Count < 1)
            {
                return (null, null, null);
            }
        }

        if (rce.ParameterList is { } pars)
        {
            ignored = pars.Parameters
                          .Where(x => ignorProps.Any(y => x.GetName().Equals(y)))
                          .ToList<SyntaxNode>();
            var parameters = pars.Parameters
                                 .Where(x => !ignorProps.Any(y => x.GetName().Equals(y)))
                                 .ToArray();
            membersData.AddRange(parameters);
            dd = parameters.Select(x => $"{GetAttributes(x)}public {x.Type} {x.GetName()} {{get;init;}}")
                           .ToList();
        }

        return (membersData, dd, ignored);
    }

    private string GetAttributes(ParameterSyntax parameterSyntax)
    {
        return parameterSyntax.AttributeLists.Select(x => x.ToString()).PipeO(x => string.Join(" ", x));
    }

    protected DtoGenerator.SourceData GenerateSyntax(string namespaceName
                                                   , string dtoName
                                                   , IEnumerable<string> propertyDescriptions
                                                   , IEnumerable<string>  propertyNames
                                                   , List<string>? ignoredPropsSyntax)
    {
        var builder = new SourceBuilder();
        builder.WriteLine("using System.ComponentModel;")
               .WriteLine("using System.ComponentModel.DataAnnotations;")
               .WriteLine($"namespace {namespaceName}")
               .OpenBrace()
               .WriteLine($"public {GetDataType()} {dtoName}")
               .OpenBrace()
               .WriteLines(propertyDescriptions)
               .WriteLinesWithOffset(AddOperators(propertyNames
                                                , ignoredPropsSyntax
                                                , dtoName
                                                , _currentNodeName))
               .WriteLinesWithOffset(AddOperators(propertyNames
                                                , Array.Empty<string>()
                                                , _currentNodeName
                                                , dtoName))
               .WriteLinesWithOffset(AddProps(propertyNames, dtoName, _currentNodeName, true))
               .WriteLinesWithOffset(AddProps(propertyNames, dtoName, _currentNodeName, false))
               .CloseBrace()
               .WriteLinesWithOffset(GenerateExtensions(dtoName, _currentNodeName))
               .CloseBrace()
               .WriteLine("");
        return new DtoGenerator.SourceData(dtoName, builder.ToString());
    }
}
