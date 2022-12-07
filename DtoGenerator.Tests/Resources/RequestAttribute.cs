using Orders.CodeGen.Infra;

namespace CodeGen.Tests.Resources;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class RequestAttribute : Attribute,IPropertyConfig
{
    public Guid GuidConstructor => Guid.NewGuid();
    public DateTime DateTimeConstructor => DateTime.Now;
    public DateTimeOffset DateTimeOffsetConstructor => DateTimeOffset.Now;
    public RequestAttribute(string prefix, params string[] hideProps)
    { }
}
