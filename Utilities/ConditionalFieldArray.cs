using System;

/// <summary>
/// Wrapped for an array to be used in conjunction with MyBox's ConditionalField attribute so that arrays can be toggled on and off in Inspector.
/// (MyBox does not support arrays with this attribute, but it does support objects.  This is an okay workaround)
/// </summary>
/// <typeparam name="T">Type of array</typeparam>
[Serializable]
public class ConditionalFieldArray<T>
{
    public T[] array;
}
