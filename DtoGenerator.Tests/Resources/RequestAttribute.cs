using DtoGenerator.Infra;

namespace DtoGenerator.Tests.Resources;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class RequestAttribute : Attribute,IPropertyConfig
{
    public RequestAttribute(string prefix, params string[] hideProps)
    { }

    public DateTime DateTimeConstructor => DateTime.Now;
    public DateTimeOffset DateTimeOffsetConstructor => DateTimeOffset.Now;
    public Guid GuidConstructor => Guid.NewGuid();
}
