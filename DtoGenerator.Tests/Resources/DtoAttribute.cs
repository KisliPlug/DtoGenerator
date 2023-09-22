using DtoGenerator.Generator;

namespace DtoGenerator.Tests.Resources;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class DtoAttribute : Attribute,IPropertyConfig
{
    public DtoAttribute(string prefix, params string[] hideProps)
    { }


}
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ContractAttribute : Attribute,IPropertyConfig
{
    public ContractAttribute(string prefix, params string[] hideProps)
    { }


}
