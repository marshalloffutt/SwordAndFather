using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Options;
using SwordAndFather.Models;

namespace SwordAndFather.Data
{
    public class UserRepository
    {
        readonly TargetRepository _targetRepository;
        readonly string _connectionString;

        public UserRepository(TargetRepository targetRepository, IOptions<DbConfiguration> dbConfig)
        {
            _targetRepository = targetRepository;
            _connectionString = dbConfig.Value.ConnectionString;
        }

        public User AddUser(string username, string password)
        {
            using (var db = new SqlConnection(_connectionString))
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

        public void DeleteUser(int userId)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var parameter = new { Id = userId };

                var deleteQuery = "Delete From Users where Id = @id";

                var rowsAffected = db.Execute(deleteQuery, parameter);

                if (rowsAffected != 1)
                {
                    throw new Exception("You done goofed");
                }
            }
        }

        public User UpdateUser(User userToUpdate)
        {
            using (var db = new SqlConnection(_connectionString))
            {
                // Nathan's preferred method
                var rowsAffected = db.Execute(@"Update Users
                                             Set username = @username,
                                                 password = @password
                                             Where id = @id", userToUpdate);

                if (rowsAffected == 1)
                    return userToUpdate;
            }
            throw new Exception("Could not update user");
        }

        public IEnumerable<User> GetAll()
        {
            using (var db = new SqlConnection(_connectionString))
            {
                var users = db.Query<User>("select username,password,id from users").ToList();

                var targets = db.Query<Target>("Select * from Targets").ToList();

                foreach (var user in users)
                {
                    user.Targets = targets.Where(target => target.UserId == user.Id).ToList();
                }

                //var targets = new TargetRepository().GetAll().GroupBy(target => target.UserId);

                //foreach (var user in users)
                //{
                //    var matchingTargets = targets.FirstOrDefault(grouping => grouping.Key == user.Id);

                //    user.Targets = matchingTargets?.ToList();
                //}

                return users;
            }
        }

    }
}