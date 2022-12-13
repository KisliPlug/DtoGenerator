using DtoGenerator.Generator;

namespace DtoGenerator.Tests.Resources;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class RequestAttribute : Attribute,IPropertyConfig
{
    public RequestAttribute(string prefix, params string[] hideProps)
    { }


}
