using Microsoft.Data.SqlClient;
using Models;

namespace Repositories
{
    public class UserRepository
    {
        private readonly string connString;

        public UserRepository(string connString)
        {
            this.connString = connString
                ?? throw new ArgumentNullException(nameof(connString));
        }

        public List<User> GetUsers()
        {
            var users = new List<User>();

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("SELECT id, name, password, email FROM [USER];", conn))
            {
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Password = reader.GetString(2),
                            Email = reader.GetString(3)
                        });
                    }
                }
            }

            return users;
        }

        public void AddUser(User user)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(
                "INSERT INTO [USER] (name, password, email) VALUES (@Name, @Password, @Email);", conn))
            {
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}