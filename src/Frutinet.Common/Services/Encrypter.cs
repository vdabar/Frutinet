using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Frutinet.Common.Extensions;
using System.Threading.Tasks;
using Frutinet.Common.Exceptions;

namespace Frutinet.Common.Services
{
    public class Encrypter : IEncrypter
    {
        private static readonly string[] InvalidCharacters =
        {
            "+", "/", "\\", "=", "-", "_", "&", "?", ",", ".", ";", " ", "<", ">", "~", "!",
            ":", "'", "\"", "[", "]", "{", "}", "|", "%", "#", "$", "^", "8", "(", ")"
        };

        private static readonly int SaltSize = 40;
        private static readonly int MinSecureKeySize = 40;
        private static readonly int MaxSecureKeySize = 60;
        private static readonly int DeriveBytesIterationsCount = 10000;
        private static readonly Random Random = new Random();

        public string GetRandomSecureKey()
        {
            var size = Random.Next(MinSecureKeySize, MaxSecureKeySize);
            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                var base64String = Convert.ToBase64String(bytes);
                var stringBuilder = new StringBuilder(base64String);
                foreach (var invalidCharacter in InvalidCharacters)
                {
                    stringBuilder.Replace(invalidCharacter, string.Empty);
                }

                return stringBuilder.ToString();
            }
        }

        public string GetSalt(string value)
        {
            if (value.Empty())
                throw new ApiException("Can not generate salt from empty value.", 400);

            var random = new Random();
            var saltBytes = new byte[SaltSize];

            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        public string GetHash(string value, string salt)
        {
            if (value.Empty())
                throw new ApiException("Can not generate hash an empty value.", 400);
            if (salt.Empty())
                throw new ApiException("Can not use an empty salt from hashing value.", 400);

            var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), DeriveBytesIterationsCount);

            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}