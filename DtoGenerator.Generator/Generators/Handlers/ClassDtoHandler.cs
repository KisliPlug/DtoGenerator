using DtoGenerator.Generator.Generators.Handlers.Interfaces;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Generators.Handlers;

public class ClassDtoHandler : AbstractDtoHandler<ClassDeclarationSyntax>
{
    public override DtoGenerator.SourceData GenerateLogic(ClassDeclarationSyntax syntax, DtoGeneratorAttribute attr)
    {
        if (!CollectAttributeAgrs(attr, out List<string> attrValues))
        {
            return Empty();
        }

        var (ignoredProps, newName, namespaceName) = GetCreateionData(syntax, attrValues);
        var allProps = syntax.Members;
        var ignoredPropsSyntax = allProps
                                .Where(x => ignoredProps.Any(y => x.ToFullString().Split(' ').Any(z => z.Equals(y))))
                                .Select(AddIgnoredPropDefaultValueCreator)
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList();
        var fields = allProps
                    .Where(x => !ignoredProps.Any(y => x.ToFullString().Split(' ').Any(z => z.Equals(y))))
                    .ToList();
        if (fields.Count < 1)
        {
            return Empty();
        }

        var names = fields.GetNodeNames().ToArray();

        // var builder = new SourceBuilder();
        //
        // builder.WriteLine("using System.ComponentModel;")
        //        .WriteLine("using System.ComponentModel.DataAnnotations;")
        //        .WriteLine($"namespace {namespaceName}")
        //        .OpenBrace()
        //        .WriteLine($"public class {newName}")
        //        .OpenBrace()
        //        .WriteLines(fields.Select(x => x.ToString()))
        //        .WriteLinesWithOffset(AddOperators(names
        //                                         , ignoredPropsSyntax
        //                                         , newName
        //                                         , hostClassName))
        //        .WriteLinesWithOffset(AddOperators(names
        //                                         , ArraySegment<string>.Empty
        //                                         , hostClassName
        //                                         , newName ))
        //        .WriteLinesWithOffset(AddProps(names, hostClassName, true))
        //        .WriteLinesWithOffset(AddProps(names, hostClassName, false))
        //        .CloseBrace()
        //        .WriteLinesWithOffset(GenerateExtensions(newName, hostClassName))
        //        .CloseBrace()
        //        .WriteLine("");
        // return new DtoGenerator.SourceData(newName, builder.ToString());
        return GenerateSyntax(namespaceName
                            , newName
                            , fields.Select(x => x.ToString())
                            , names
                            , ignoredPropsSyntax);
    }

    protected override string GetCreateNewInstanceBracket(bool isOpen, string entityName) => isOpen ? "{" : "};";

    protected override string AssignConstructorSymbol(string entityName) => "=";
    protected override string GetDataType() => "class";

    protected override IEnumerable<string> AddProps(IEnumerable<string> fields, string name, string dtoName, bool set)
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
}
