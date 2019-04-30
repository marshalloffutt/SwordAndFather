using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using SwordAndFather.Models;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        const string ConnectionString = "Server=localhost;Database=SwordAndFather;Trusted_Connection=True;";

        public User AddUser(string username, string password)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var newUser = db.QueryFirstOrDefault<User>(@"
                    insert into Users(Username, Password)
                    output inserted.*
                    values(@username, @password)", 
                    new { username, password}); // <--- anonymous type

                if (newUser != null)
                {
                    return newUser;
                }
            }

            throw new Exception("No user created");
        }

        public void DeleteUser(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var rowsAffected = db.Execute("Delete From Users where Id = @id", new {id});

                if (rowsAffected != 1)
                {
                    throw new Exception("You done goofed");
                }
            }
        }

        public User UpdateUser(User userToUpdate)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                var rowsAffected = db.Execute(@"Update Users
                                             Set username = @username,
                                                 password = @password
                                             Where id = @id", userToUpdate);

                if (rowsAffected == 1)
                    return userToUpdate;
            }
            throw new Exception("Could not update user");
        }

        public List<User> GetAll()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                return db.Query<User>("select username,password,id from users").ToList();
            }
        }

    }
}