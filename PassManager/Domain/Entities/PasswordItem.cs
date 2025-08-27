using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Domain.Entities
{
    public class PasswordItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }            // e.g., "Gmail"
        public string? Username { get; set; }         // account username
        public string? EncryptedPassword { get; set; } // encrypted bytes base64
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
