using Services;
using Repository;

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

            var app = builder.Build();

            app.MapRazorPages();

            app.Run();
        }
    }
}
