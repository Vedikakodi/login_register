using System;

namespace DataStore.Abstraction.Models
{
    public class User
    {
        public int Id { get; set; }  // Auto-incremented by SQL
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

}
