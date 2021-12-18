using Npgsql;
using project_sem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_sem.Database
{
    public class MyDatabase
    {
        private static string ConnectionString =
              "User ID=postgres; Server=localhost; port=5433; Database=project; Password=1234; Pooling=true;";

        private static NpgsqlConnection Connection = new NpgsqlConnection(ConnectionString);

        private static string UserProperties = "id, username, password, email";
        private static string ProfileProperties = "Id, City, Description";


        private static string UserTable = "Users";
        private static string ProfileTable = "Profile";

        public static async Task Add(User user)
        {
            await Connection.OpenAsync();

            var userValues = GetValues(user);
            var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task Add(Profile profile)
        {
            await Connection.OpenAsync();

            var profileValues = GetValues(profile);
            var comm = $"INSERT INTO \"{ProfileTable}\" ({ProfileProperties}) VALUES ({profileValues});";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        public static async Task AddRange(IEnumerable<User> users)
        {
            await Connection.OpenAsync();

            foreach (var user in users)
            {
                var userValues = GetValues(user);
                var comm = $"INSERT INTO \"{UserTable}\" ({UserProperties}) VALUES ({userValues});";
                var cmd = new NpgsqlCommand(comm, Connection);
                await cmd.ExecuteNonQueryAsync();
            }

            await Connection.CloseAsync();
        }

        public static async Task<List<User>> GetAllUsers()
        {
            await Connection.OpenAsync();

            var users = new List<User>();

            var cmd = new NpgsqlCommand($"SELECT * FROM \"{UserTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User()
                {
                    id = reader.GetGuid(0),
                    username = reader.GetString(1),
                    email = reader.GetString(2),
                    password = reader.GetString(3),
                });
            }
            await Connection.CloseAsync();

            return users;
        }

        public static async Task<List<Models.Profile>> GetAllProfiles()
        {
            await Connection.OpenAsync();

            var profiles = new List<Models.Profile>();

            var cmd = new NpgsqlCommand($"SELECT * FROM \"{ProfileTable}\"", Connection);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                profiles.Add(new Profile()
                {
                    Id = reader.GetGuid(0),
                    City = reader.GetString(1),
                    Description = reader.GetString(2),
                });
            }
            await Connection.CloseAsync();

            return profiles;
        }

        public static async void Update(Models.Profile profile)
        {
            await Connection.OpenAsync();

            var profileValues = GetValues(profile);
            var comm = $"UPDATE \"{ProfileTable}\" SET ({ProfileProperties}) = ({profileValues}) WHERE id='{profile.Id}'";
            var cmd = new NpgsqlCommand(comm, Connection);
            await cmd.ExecuteNonQueryAsync();

            await Connection.CloseAsync();
        }

        private static string GetValues(User user) =>
            $"'{user.id}', '{user.username}', '{user.email}', '{user.password}'";
        private static string GetValues(Models.Profile profile) =>
            $"'{profile.Id}', '{profile.City}', '{profile.Description}'";
    }
}
