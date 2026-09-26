using Services;
using Repositories;

namespace Site_de_Receitas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();

            var connString = builder.Configuration.GetConnectionString("Receita")
                ?? throw new InvalidOperationException("ConnectionStrings:Receita não configurada.");

            builder.Services.AddScoped(_ => new UserRepository(connString));
            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped(_ => new IngredientRepository(connString));
            builder.Services.AddScoped<IngredientsServices>();

            var app = builder.Build();

            app.MapRazorPages();

            app.Run();
        }
    }
}