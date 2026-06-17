namespace Solidcode.Work.Infra.Abstractions;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class AggregateRootAttribute : Attribute
{
    public Type RootType { get; }

    public AggregateRootAttribute(Type rootType)
    {
        RootType = rootType;
    }
}