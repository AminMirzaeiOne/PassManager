using PassManager.Domain.Entities;
using PassManager.Domain.Interfaces;
using PassManager.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassManager.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) { _repo = repo; }

        public async Task<bool> AnyUserAsync() => await _repo.AnyUserAsync();

        public async Task CreateUserAsync(string username, string password)
        {
            var passSalt = CryptoService.GenerateSalt();
            var keySalt = CryptoService.GenerateSalt();
            var iterations = 100_000;
            var hash = CryptoService.HashPassword(password, passSalt, iterations);
            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                PasswordSalt = passSalt,
                KeySalt = keySalt,
                Iterations = iterations
            };
            await _repo.AddAsync(user);
        }

        public async Task<(bool ok, User user)> AuthenticateAsync(string username, string password)
        {
            var user = await _repo.GetByUsernameAsync(username);
            if (user == null) return (false, null);
            var hash = CryptoService.HashPassword(password, user.PasswordSalt, user.Iterations);
            var ok = hash.SequenceEqual(user.PasswordHash);
            return (ok, ok ? user : null);
        }

        public byte[] DeriveAesKeyForSession(string password, byte[] keySalt, int keySize = 32)
            => CryptoService.DeriveKey(password, keySalt, keySize);
    }
}
