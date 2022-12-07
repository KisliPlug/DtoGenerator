using DtoGenerator.Generator.Generators.Handlers.Interfaces;
using DtoGenerator.Generator.Recievers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Generators.Handlers;

public class RecordDtoHandler : AbstractDtoHandler<RecordDeclarationSyntax>
{
    public override DtoGenerator.SourceData Generate(RecordDeclarationSyntax syntax, DtoGeneratorAttribute attr)
    {
        if (!CollectAttributeAgrs(attr, out List<string> attrValues))
        {
            return Empty();
        }

        var (ignoredNames, newName, namespaceName) = GetCreateionData(syntax, attrValues);
        var (membersData, propertyDescriptions, ignoredFields) = CollectRecordFields(syntax, ignoredNames);
        if (membersData is null || propertyDescriptions is null)
        {
            return Empty();
        }

        var names = membersData.GetNodeNames().ToArray();
        var hostRecordName = syntax.Identifier.ToString();


        var builder = new SourceBuilder();

        builder.WriteLine("using System.ComponentModel;")
               .WriteLine("using System.ComponentModel.DataAnnotations;")
               .WriteLine($"namespace {namespaceName}")
               .OpenBrace()
               .WriteLine($"public record {newName}")
               .OpenBrace()
               .WriteLines(propertyDescriptions)
               .WriteLinesWithOffset(AddOperators(names
                                                 , ignoredFields
                                                 , newName
                                                 , hostRecordName
                                                 , true))
               .WriteLinesWithOffset(AddOperators(names
                                                 , ignoredFields
                                                 , newName
                                                 , hostRecordName
                                                 , false))
               .WriteLinesWithOffset(AddPropsRecord(names, newName, hostRecordName,true))
               .WriteLinesWithOffset(AddPropsRecord(names, newName, hostRecordName,false))
               .CloseBrace()
               .WriteLinesWithOffset(GenerateExtensions(newName, hostRecordName))
               .CloseBrace()
               .WriteLine("");
         return new DtoGenerator.SourceData( newName, builder.ToString());

    }
}
