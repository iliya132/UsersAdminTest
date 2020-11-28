using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using UsersTest.Models.Entities;
using UsersTest.Models.Interfaces;

namespace UsersTest.Models.Implementations
{
    public class SQLiteDataProvider : IDataProvider
    {
        private SqliteConnection _connection = new SqliteConnection("DataSource=users.db");
        public SQLiteDataProvider()
        {
            EnsureCreated();
        }

        private void EnsureCreated()
        {
            SqliteCommand command = new SqliteCommand(
                @"CREATE TABLE IF NOT EXISTS 'Users' (
                'Id'    INTEGER NOT NULL UNIQUE,
                'Login' TEXT NOT NULL,
                'Name'  TEXT NOT NULL,
                'Password'  TEXT NOT NULL,
                'Email' TEXT NOT NULL,
                PRIMARY KEY('Id' AUTOINCREMENT)
                );", _connection);
            command.ExecuteNonQuery();
            command = new SqliteCommand(
                @"CREATE TABLE IF NOT EXISTS 'Roles'(
                'Id'    INTEGER NOT NULL UNIQUE,
                'Name'  TEXT NOT NULL UNIQUE,
                PRIMARY KEY('Id' AUTOINCREMENT)
                );", _connection);
            command.ExecuteNonQuery();
            command = new SqliteCommand(
                @"CREATE TABLE IF NOT EXISTS 'UserRoles' (
                'Id'    INTEGER NOT NULL UNIQUE,
                'UserId'    INTEGER NOT NULL,
                'RoleId'    INTEGER NOT NULL,
                FOREIGN KEY('RoleId') REFERENCES 'Roles'('Id'),
                PRIMARY KEY('Id' AUTOINCREMENT),
                FOREIGN KEY('UserId') REFERENCES 'Users'('Id')
                );", _connection);
            command.ExecuteNonQuery();
            command = new SqliteCommand(
                "select count(*) from Roles");
            int rolesCount = (int)command.ExecuteScalar();
            if(rolesCount < 1)
            {
                command = new SqliteCommand(
                    @"INSERT into Roles (name) values ('Admin');
                    INSERT into Roles(name) values('User');"
                    );
                command.ExecuteNonQuery();
            }
        }

        public int AddUser(User newUser)
        {
            _connection.Open();
            int newId = 0;
            try
            {
                //Здесь нужна реализация защиты от SQL инъекций. Упускаю т.к. тестовое задание
                string sql = $@"INSERT into Users (Login, name, Email, Password) values ({newUser.Login}, {newUser.Name}, {newUser.Email}, {newUser.Password});
                        SELECT last_insert_rowid();";
                SqliteCommand command = new SqliteCommand(sql);
                newId = (int)command.ExecuteScalar();
            }
            finally
            {
                _connection.Close();
            }
            return newId;
        }

        public bool DeleteUser(User deletedUser)
        {
            throw new NotImplementedException();
        }

        public bool EditUser(User editedUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
