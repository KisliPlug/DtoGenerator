using System.Collections;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DtoGenerator.Generator.Recievers;

public static class NodeExtensions
{
    public static T GetParent<T>(this SyntaxNode node)
    {
        var parent = node.Parent;
        while (true)
        {
            if (parent == null)
            {
                return default;
            }

            if (parent is T t)
            {
                return t;
            }

            parent = parent.Parent;
        }
    }

    public static IEnumerable<string> GetNodeNames(this IEnumerable<SyntaxNode> nodes)
    {
        return nodes.Select(GetName);
    }

    public static string GetName(this SyntaxNode syntaxNode)
    {
        return syntaxNode switch
               {
                   ClassDeclarationSyntax cl    => cl.Identifier.Text
                 , TypeDeclarationSyntax tp     => tp.Identifier.Text
                 , PropertyDeclarationSyntax tp => tp.Identifier.Text
                 , MemberDeclarationSyntax mb   => mb.ToString()
                 , ParameterSyntax mb           => mb.Identifier.ToString()
                 , _                            => syntaxNode.ToString().Split(" ").First()
               };
    }

    public static void Log(this object data, string fileName = "debug.log")
    {
        try
        {
            var logData = new StringBuilder();
            if (data is IEnumerable list and not string)
            {
                foreach (var e in list)
                {
                    logData.Append(e.ToString() + "\n");
                }
            }
            else
            {
                logData.Append(data.ToString());
            }

            using var str = new StreamWriter(@$"F:\REPOS\Orders\DtoGenerator.Generator\Output\{fileName}", append: true);
            str.WriteLine(logData.ToString());
        }
        catch (Exception e)
        { }
    }
}
