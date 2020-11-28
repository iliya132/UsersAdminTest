using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersTest.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "TestIssuer";
        public const string AUDIENCE = "UsersTest";
        static string KEY
        {
            get
            {
                string tmp;
                using (StreamReader reader = new StreamReader(key_path))
                {
                    tmp = reader.ReadToEnd();
                }
                return tmp;
            }
        }
        const string key_path = "Key.txt";
        public const int LIFETIME = 10;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
