using System.Collections;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq;
using Orders.CodeGen.Recievers;

namespace Orders.CodeGen.Generators.Handlers.Interfaces;

public abstract class AbstractDtoHandler<T> : IDtoHandler<T> where T : SyntaxNode
{
    protected DtoGenerator.SourceData Empty()
    {
        return new DtoGenerator.SourceData("", "");
    }

    public abstract DtoGenerator.SourceData Generate(T syntax, DtoGeneratorAttribute attr);

    protected IEnumerable<string> GenerateExtensions(string newName, string hostClassName)
    {
        yield return $"public static class {newName}Extensions";
        yield return "{";
        yield return $"public static {newName} As{newName}(this {hostClassName} entity)";
        yield return "{";
        yield return $"return ({newName})entity;";
        yield return "}";
        ;
        yield return $"public static {hostClassName} As{hostClassName}(this {newName} dto)";
        yield return "{";
        yield return $"return ({hostClassName})dto;";
        yield return "}";
        yield return $"public static IEnumerable<{newName}> As{newName}(this IEnumerable<{hostClassName}> entity)";
        yield return "{";
        yield return $"return  entity.Select(x=>x.As{newName}());";
        yield return "}";
        yield return $"public static IEnumerable<{hostClassName}> As{hostClassName}(this IEnumerable<{newName}> dtos)";
        yield return "{";
        yield return $"return dtos.Select(x=>x.As{hostClassName}());";
        yield return "}";
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


    protected IEnumerable<string> AddOperators(IEnumerable<string> fields, IEnumerable<SyntaxNode> ignored, string from, string to, bool fromDto)
    {
        var dataFields = fields
                        .Select(x => $"{x}=b.{x}")
                        .ToList();
        if (!fromDto)
        {
            foreach (var s in ignored.Select(AddIgnoredPropDefaultValueCreator).Where(x => !string.IsNullOrEmpty(x)))
            {
                dataFields.Add(s);
            }
        }

        AddSeparator(dataFields);
        var toStr = fromDto ? from : to;
        var fromStr = fromDto ? to : from;
        yield return $"public static explicit operator {toStr}({fromStr} b)";
        yield return "{";
        yield return $"return new {toStr}";
        yield return "{";
        foreach (var dd in dataFields)
        {
            yield return dd;
        }

        yield return "};";
        yield return "}";
    }

    private void AddSeparator(List<string> dataFields)
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
            propType = parameter.Type.ToString();
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

        return $"{name}={add}";
    }

    protected IEnumerable<string> AddPropsRecord(IEnumerable<string> fields, string pocoName, string dtoName, bool set)
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

        var retType = set ? dtoName : pocoName;
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

        //         return $@"
        //      public {retType} {funcPrefix}Props({dtoName} dto)
        //     {{
        //
        //            return  {retInditefer} with {{{string.Join("\t\n,", dataFields)}}};
        //
        //     }}";
    }

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

    protected IEnumerable<string> AddProps(IEnumerable<string> fields, string name, bool set)
    {
        string setProp(string propName)
        {
            return $"b.{propName}={propName};";
        }

        string getProp(string propName)
        {
            return $"{propName}=b.{propName};";
        }

        var funcPrefix = set ? "Set" : "Get";
        Func<string, string> func = set ? setProp : getProp;
        var dataFields = fields
           .Select(x => func(x));
        yield return $"public   void  {funcPrefix}Props({name} b)";
        yield return "{";
        foreach (var field in dataFields)
        {
            yield return field;
        }

        yield return "}";
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
                     .Where(x => ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
                     .ToList<SyntaxNode>();
            membersData = members
                         .Where(x => !ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
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
                          .Where(x => ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
                          .ToList<SyntaxNode>();
            var parameters = pars.Parameters
                                 .Where(x => !ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
                                 .ToArray();
            parameters.ForEach(x => membersData.Add(x));
            dd = parameters.Select(x => $"{GetAttributes(x)}public {x.Type} {x.GetName()} {{get;init;}}")
                           .ToList();
        }

        return (membersData, dd, ignored);
    }

    private string GetAttributes(ParameterSyntax parameterSyntax)
    {
        return parameterSyntax.AttributeLists.Select(x => x.ToString()).PipeO(x => string.Join(" ", x));
    }
}
