using System;

namespace WilliamsonFamily.Models.Family
{
    public interface IFamilyRepository : IModelLoader<IFamily, string>
    {
		IFamily LoadFromUsername(string username);
    }
}