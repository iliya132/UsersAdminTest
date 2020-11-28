﻿using Microsoft.IdentityModel.Tokens;

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
        public const string ISSUER = "TestIssuer"; // издатель токена
        public const string AUDIENCE = "UsersTest"; // потребитель токена
        static string KEY;   // ключ для шифрации
        const string key_path = "Key.txt";
        public const int LIFETIME = 1; // время жизни токена - 1 минута
        static AuthOptions()
        {

            using (StreamReader reader = new StreamReader(key_path))
            {
                KEY = reader.ReadToEnd();
            }
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}