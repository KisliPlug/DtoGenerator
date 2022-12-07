using DtoGenerator.Generator.Generators.Handlers.Interfaces;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Generators.Handlers;

public class ClassDtoHandler : AbstractDtoHandler<ClassDeclarationSyntax>
{
    public override DtoGenerator.SourceData Generate(ClassDeclarationSyntax syntax, DtoGeneratorAttribute attr)
    {
        if (!CollectAttributeAgrs(attr, out List<string> attrValues))
        {
            return Empty();
        }

        var (ignorProps, newName, namespaceName) = GetCreateionData(syntax, attrValues);
        var allProps = syntax.Members;
        var ignorPropsSyntax = allProps
                              .Where(x => ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
                              .ToList();
        var fields = allProps
                    .Where(x => !ignorProps.Any(y => x.ToFullString().Split(" ").Any(z => z.Equals(y))))
                    .ToList();
        if (fields.Count < 1)
        {
            return Empty();
        }

        var hostClassName = syntax.Identifier.ToString();
        var names = fields.GetNodeNames().ToArray();
        var builder = new SourceBuilder();

        builder.WriteLine("using System.ComponentModel;")
               .WriteLine("using System.ComponentModel.DataAnnotations;")
               .WriteLine($"namespace {namespaceName}")
               .OpenBrace()
               .WriteLine($"public class {newName}")
               .OpenBrace()
               .WriteLines(fields.Select(x => x.ToString()))
               .WriteLinesWithOffset(AddOperators(names
                                                 , ignorPropsSyntax
                                                 , newName
                                                 , hostClassName
                                                 , true))
               .WriteLinesWithOffset(AddOperators(names
                                                 , ignorPropsSyntax
                                                 , newName
                                                 , hostClassName
                                                 , false))
               .WriteLinesWithOffset(AddProps(names, hostClassName, true))
               .WriteLinesWithOffset(AddProps(names, hostClassName, false))
               .CloseBrace()
               .WriteLinesWithOffset(GenerateExtensions(newName, hostClassName))
               .CloseBrace()
               .WriteLine("");

        return new DtoGenerator.SourceData(newName, builder.ToString());
    }
}
