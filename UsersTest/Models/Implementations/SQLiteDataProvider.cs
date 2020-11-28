using System.Data.SQLite;

using System.Collections.Generic;
using System.Text;

using UsersTest.Models.Entities;
using UsersTest.Models.Interfaces;
using System;
using System.Security.Cryptography;

namespace UsersTest.Models.Implementations
{
    public class SQLiteDataProvider : IDataProvider
    {
        private SQLiteConnection _connection = new SQLiteConnection("DataSource=users.db");
        public SQLiteDataProvider()
        {
            _connection.Open();
            EnsureCreated();
        }

        private void EnsureCreated()
        {
            SQLiteCommand command = new SQLiteCommand(
                @"CREATE TABLE IF NOT EXISTS 'Users' (
                'Id'    INTEGER NOT NULL UNIQUE,
                'Login' TEXT NOT NULL,
                'Name'  TEXT NOT NULL,
                'Password'  TEXT NOT NULL,
                'Email' TEXT NOT NULL,
                PRIMARY KEY('Id' AUTOINCREMENT)
                );", _connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand(
                @"CREATE TABLE IF NOT EXISTS 'Roles'(
                'Id'    INTEGER NOT NULL UNIQUE,
                'Name'  TEXT NOT NULL UNIQUE,
                PRIMARY KEY('Id' AUTOINCREMENT)
                );", _connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand(
                @"CREATE TABLE IF NOT EXISTS 'UserRoles' (
                'Id'    INTEGER NOT NULL UNIQUE,
                'UserId'    INTEGER NOT NULL,
                'RoleId'    INTEGER NOT NULL,
                FOREIGN KEY('RoleId') REFERENCES 'Roles'('Id'),
                PRIMARY KEY('Id' AUTOINCREMENT),
                FOREIGN KEY('UserId') REFERENCES 'Users'('Id')
                );", _connection);
            command.ExecuteNonQuery();
            command = new SQLiteCommand(
                "select count(*) from Roles", _connection);
            int rolesCount = Convert.ToInt32(command.ExecuteScalar());
            if(rolesCount < 1)
            {
                command = new SQLiteCommand(
                    @"INSERT into Roles (name) values ('Admin');
                    INSERT into Roles(name) values('User');", _connection
                    );
                command.ExecuteNonQuery();
            }
        }

        string GetHashString(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        public int AddUser(User newUser)
        {
            long newId;
            //Здесь нужна реализация защиты от SQL инъекций. Упускаю т.к. тестовое задание
            string sql = $@"INSERT into Users (Login, name, Email, Password) values ('{newUser.Login}', '{newUser.Name}', '{newUser.Email}', '{GetHashString(newUser.Password)}');
                    SELECT last_insert_rowid();";
            SQLiteCommand command = new SQLiteCommand(sql, _connection);
                newId = (long)command.ExecuteScalar();
            StringBuilder sb = new StringBuilder();
            foreach(Role role in newUser.Roles)
            {
                sb.Append($"INSERT into UserRoles (UserId, RoleId) values ({newId}, {role.Id});");
            }
            new SQLiteCommand(sb.ToString(), _connection).ExecuteNonQuery();
            return (int)newId;
        }

        public bool DeleteUser(User deletedUser)
        {
            string sql = $"delete from Users where Users.Id = {deletedUser.Id}";
            SQLiteCommand command = new SQLiteCommand(sql, _connection);
            command.ExecuteNonQuery();
            return true;
        }

        public bool EditUser(User editedUser)
        {
            try
            {
                //Здесь нужна реализация защиты от SQL инъекций. Упускаю т.к. тестовое задание
                string sql = $@"update Users SET
                            name = '{editedUser.Name}',
                            login = '{editedUser.Login}',
                            Email = '{editedUser.Email}',
                            Password = '{editedUser.Password}'
                            where Users.Id = {editedUser.Id}";
                SQLiteCommand command = new SQLiteCommand(sql, _connection);
                command.ExecuteNonQuery();
                sql = $"delete from UserRoles where UserId = {editedUser.Id}";
                new SQLiteCommand(sql, _connection).ExecuteNonQuery();
                StringBuilder sb = new StringBuilder();
                foreach (Role role in editedUser.Roles)
                {
                    sb.Append($"INSERT into UserRoles (UserId, RoleId) values ({editedUser.Id}, {role.Id});");
                }
                command = new SQLiteCommand(sb.ToString(), _connection);
                command.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            try
            {
                string sql = "select * from Roles";
                SQLiteCommand command = new SQLiteCommand(sql, _connection);
                SQLiteDataReader reader = command.ExecuteReader();
                List<Role> rolesResult = new List<Role>();
                while (reader.Read())
                {
                    rolesResult.Add(new Role
                    {
                        Id = (int)(long)reader["Id"],
                        Name = (string)reader["Name"]
                    });
                }
                return rolesResult;
            }
            catch
            {
                return new List<Role>();
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                List<User> usersResult = new List<User>();
                string sql = "select * from Users";
                SQLiteCommand command = new SQLiteCommand(sql, _connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    User usr = new User
                    {
                        Id = (int)(long)reader["Id"],
                        Name = (string)reader["Name"],
                        Login = (string)reader["Login"],
                        Email = (string)reader["Email"]
                    };
                    sql = $@"select UserId, RoleId, Roles.Id, Roles.Name from UserRoles
                            INNER JOIN Roles on RoleId = Roles.Id
                            where UserId = {usr.Id}";
                    SQLiteCommand getRolescommand = new SQLiteCommand(sql, _connection);
                    SQLiteDataReader rolesReader = getRolescommand.ExecuteReader();
                    while (rolesReader.Read())
                    {
                        Role role = new Role
                        {
                            Id = (int)(long)rolesReader["Id"],
                            Name = (string)rolesReader["Name"]
                        };
                        usr.Roles.Add(role);
                    }
                    usersResult.Add(usr);
                }
                return usersResult;
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}
