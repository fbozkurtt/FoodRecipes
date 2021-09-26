using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Controllers
{
    public class PictureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
