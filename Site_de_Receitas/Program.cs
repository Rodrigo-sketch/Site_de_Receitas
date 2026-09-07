namespace Site_de_Receitas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();


            var app = builder.Build();

            app.MapRazorPages();

            app.Run();
        }
    }
}
