using System;

namespace WilliamsonFamily.Models.Family
{
    public interface IUserFamily
    {
        string UserID { get; set; }
        int? FamilyID { get; set; }
    }
}
