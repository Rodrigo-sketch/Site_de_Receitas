using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using Models;

namespace Repository
{
    public class UserRepository
    {
        string connString = "Server=(localdb)\\MSSQLLocalDB;Database=recipes;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("SELECT id, name, password, email FROM Users", conn);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Password = reader.GetString(2),
                    Email = reader.GetString(3)
                }
                );
            }

            conn.Close();
            
            return users;

        }
        public UserRepository() { }
    }
}
