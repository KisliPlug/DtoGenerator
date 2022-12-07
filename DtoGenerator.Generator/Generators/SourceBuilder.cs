using System.Text;

namespace Orders.CodeGen.Generators;

public class SourceBuilder
{
    private readonly StringBuilder _stringBuilder;
    private int _indentLevel;
    public const string Tab = "    ";
    public const string CloseBracket = "}";
    public const string OpenBracket = "{";

    public SourceBuilder()
    {
        _stringBuilder = new StringBuilder();
        _indentLevel = 0;
    }

    public SourceBuilder WriteLine(string line)
    {
        _stringBuilder.AppendLine($"{AddTabs()}{line}");
        return this;
    }

    public SourceBuilder WriteLines(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            this.WriteLine(line);
        }

        return this;
    }

    public SourceBuilder OpenBrace()
    {
        _stringBuilder.AppendLine($"{AddTabs()}{{");
        _indentLevel++;
        return this;
    }

    public SourceBuilder CloseBrace(bool addSemicolumn = false)
    {
        _indentLevel--;
        var semicol = addSemicolumn ? ";" : "";
        _stringBuilder.AppendLine($"{AddTabs()}}}{semicol}");
        return this;
    }

    private string AddTabs()
    {
        if (_indentLevel > 0)
        {
            return string.Concat(Enumerable.Repeat(Tab, _indentLevel));
        }

        return "";
    }

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }

    public IEnumerable<string> AsList()
    {
        return _stringBuilder.ToString().Split("\r").SelectMany(x => x.Split("\n")).Where(x => x.Length > 0);
    }

    public SourceBuilder WriteLinesWithOffset(IEnumerable<string> generateExtensions)
    {
        foreach (var str in generateExtensions)
        {
            if (str.Contains(CloseBracket) && !str.Contains(OpenBracket))
            {
                _indentLevel--;
                _stringBuilder.AppendLine($"{AddTabs()}{str}");
            }
            else if (!str.Contains(CloseBracket) && str.Contains(OpenBracket))
            {
                _stringBuilder.AppendLine($"{AddTabs()}{str}");
                _indentLevel++;
            }
            else
            {
                _stringBuilder.AppendLine($"{AddTabs()}{str}");
            }
        }

        return this;
    }
}
