using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frutinet.Common.Services
{
    public interface IEncrypter
    {
        string GetHash(string value, string salt);

        string GetSalt(string value);

        string GetRandomSecureKey();
    }
}