using System;

namespace WilliamsonFamily.Models
{
    public interface IUniqueKey<Type>
    {
        Type UniqueKey { get; }
    }
}
