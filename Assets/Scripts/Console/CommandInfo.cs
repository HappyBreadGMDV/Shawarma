using System;
using System.Collections.Generic;
using System.Reflection;

public struct CommandInfo
{
    public object Target;           // экземпляр MonoBehaviour, на котором метод
    public MethodInfo Method;
    public List<Type> ParamTypes;

    public CommandInfo(object target, MethodInfo method, List<Type> paramTypes)
    {
        Target = target;
        Method = method;
        ParamTypes = paramTypes;
    }
}