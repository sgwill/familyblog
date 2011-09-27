using System;

namespace WilliamsonFamily.Models.Caching
{
    public interface ICacheKey
    {
        string GenerateKey(string partialKey);
    }
}