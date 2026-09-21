using Microsoft.Data.SqlClient;
using Models;

namespace Repositories
{
    public class IngredientRepository
    {
        string connString = "Server=localhost,1433;Database=Receita;User Id=sa;Password=Digo@1802;TrustServerCertificate=True;";

        public List<Ingredient> GetIngredients()
        {
            var ingredients = new List<Ingredient>();

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("SELECT Id, Name FROM INGREDIENT", conn))
            {
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredients.Add(new Ingredient
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }
            }

            return ingredients;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("INSERT INTO INGREDIENT (Name) VALUES (@Name)", conn))
            {
                cmd.Parameters.AddWithValue("@Name", ingredient.Name);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}