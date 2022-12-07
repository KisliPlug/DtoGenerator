using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeGen.Tests;

public static class Fixture
{
    public static string _dirName = "../../../../GenerationTests/";

    public static void AddSource(string fileName, string source)
    {
        if (!Directory.Exists(_dirName))
        {
            Directory.CreateDirectory(_dirName);
        }

        using var wr = new StreamWriter($"{_dirName}/{fileName}", false);
        wr.Write(source);
    }
}
