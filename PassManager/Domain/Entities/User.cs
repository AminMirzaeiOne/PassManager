using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }

        // Authentication (PBKDF2 hash)
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public int Iterations { get; set; }

        // separate salt for deriving AES key
        public byte[]? KeySalt { get; set; }
    }
}
