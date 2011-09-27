using System;

namespace WilliamsonFamily.Models.User
{
    public interface IUser : IUniqueKey<string>
    {
        string Username { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        DateTime? Birthdate { get; set; }
        string Email { get; set; }
    }
}
