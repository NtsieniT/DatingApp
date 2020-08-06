using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Data
{
    // This repository is responsible for querying database via Entity Framework
    public class AuthRepository : IAuthRepository
    {
        //Inject data context into the AuthRepository
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            /*
             * We will use username to identify the user in the database and
             * use password to compare it to the hashed passwords.
             * so we compute the hash that the password generates and compare it with the password hash 
             * that we storing in our database.
             */

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }

            /*This method return true or false depending if password matches or not.
             * If password doesn't match, return null 
             */
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
            
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                /*
                 * Create an instance of HMAC passing it a key so that it can compute the hash
                 * 
                 */ 

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                /*
                 * We looping over in each element of the byte array computedHash and calculate and
                 * compare it with each element of passwordHash.
                 * if the elements match up, the password is correct
                 */
                for (int i = 0; i <  computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            /* Pass passwordHash and passwordSalt as references so that when they are updated
             they updated on the variable as well.
            */
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Using system security cryptography for password encryption to generate a random key.

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // setting our hash and salt to randomly generated keys and storred in the bytes arrays
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            
        }

        public async Task<bool> UserExists(string username)
        {
            /*
             * Check if user exists
             */
            if (await _context.Users.AnyAsync(x => x.Username == username))
            {
                return true;
            }
            return false;
        }
    }
}
