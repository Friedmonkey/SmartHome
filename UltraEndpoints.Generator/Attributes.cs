using System;

namespace UltraEndpoints.Generator;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class UltraEndpointAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class UltraGetAttribute : Attribute
{
    public string Route { get; }

    public UltraGetAttribute(string route)
    {
        Route = route;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class UltraPostAttribute : Attribute
{
    public string Route { get; }

    public UltraPostAttribute(string route)
    {
        Route = route;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class UltraPutAttribute : Attribute
{
    public string Route { get; }

    public UltraPutAttribute(string route)
    {
        Route = route;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class UltraDeleteAttribute : Attribute
{
    public string Route { get; }

    public UltraDeleteAttribute(string route)
    {
        Route = route;
    }
}

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class UltraInjectAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class UltraBaseAttribute : Attribute
{
}
