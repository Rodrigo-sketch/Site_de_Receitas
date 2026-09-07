using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repository;



namespace Site_de_Receitas.Pages
{
    public class IndexModel : PageModel
    {

        public User user;
        public void OnGet()
        {
            UserRepository  userRepository = new UserRepository();

            user = userRepository.GetUsers().FirstOrDefault();

        }
    }
}
