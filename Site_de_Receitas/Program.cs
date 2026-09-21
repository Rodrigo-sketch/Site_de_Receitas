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

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<UserService>();

            builder.Services.AddScoped<IngredientRepository>();
            builder.Services.AddScoped<IngredientsServices>();

            var app = builder.Build();

            app.MapRazorPages();

            app.Run();
        }
    }
}