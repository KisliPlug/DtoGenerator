

using DtoGenerator.Generator;

namespace TestNuget.Resources;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class DtoAttribute : Attribute,IPropertyConfig
{
    public DtoAttribute(string prefix, params string[] hideProps)
    { }


}
