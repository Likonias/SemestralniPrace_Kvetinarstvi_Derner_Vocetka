using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components
{
    public static class PasswordHash
    {

        public static string PasswordHashing(string password)
        {

            using var sha = SHA256.Create();

            var asBytes = Encoding.UTF8.GetBytes(password);

            var hashed = sha.ComputeHash(asBytes);

            return Convert.ToBase64String(hashed);

        }

        public static bool IsPasswordCorrect(string password1, string password2)
        {
            byte[] pass1 = Convert.FromBase64String(password1);
            byte[] pass2 = Convert.FromBase64String(password2);

            return StructuralComparisons.StructuralEqualityComparer.Equals(pass1, pass2);
        }
    }
}
