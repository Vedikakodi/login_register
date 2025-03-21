using System;

namespace FeatureObjects.Abstraction.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Plain text password (hashed later)
    }

}
