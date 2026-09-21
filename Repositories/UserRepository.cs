using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using Models;

namespace Repositories
{
    public class UserRepository
    {
        //string connString = "Server=(localdb)\\MSSQLLocalDB;Database=Receita;Trusted_Connection=True;TrustServerCertificate=True;";
        string connString = "Server=localhost,1433;Database=Receita;User Id=sa;Password=Digo@1802;TrustServerCertificate=True;";
        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("SELECT id, name, password, email FROM [USER];", conn);

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

        public void AddUser(User user)
        {
            List<User> users = new List<User>();
            users.Add(user);
            throw new NotImplementedException();
        }

        public UserRepository() { }


    }
}
