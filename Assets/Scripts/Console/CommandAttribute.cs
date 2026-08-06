using System;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class CommandAttribute : Attribute
{
    public Type[] ParamTypes { get; }

    public CommandAttribute()
    {
        ParamTypes = new Type[0];
    }

    public CommandAttribute(params Type[] paramTypes)
    {
        ParamTypes = paramTypes;
    }
}