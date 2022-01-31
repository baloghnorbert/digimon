using System;

namespace Backend.Core.Interfaces.Common
{
    public interface IJsonAttribute
    {
        object TryConvert(string modelValue, Type targertType, out bool success);
    }
}
