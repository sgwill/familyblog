using System;

namespace WilliamsonFamily.Models.Family
{
    public interface IFamily : IUniqueKey<int>
    {
        string FamilyName { get; set; }
        string Description { get; set; }
    }
}
