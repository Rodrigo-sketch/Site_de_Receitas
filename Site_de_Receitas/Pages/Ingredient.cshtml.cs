using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Services;

namespace Site_de_Receitas.Pages;


public class IngredientModel : PageModel
{

private readonly IngredientsServices _ingredientServices;

public List<Ingredient> ingredients { get; set; }

public IngredientModel(IngredientsServices ingredientsServices)
{
    _ingredientServices = ingredientsServices;
}

public void OnGet()
{
    ingredients = _ingredientServices.GetAllIngredients();
}
}
