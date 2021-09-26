using Internative.FoodRecipes.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Controllers
{
    public class HomeController : BasePublicController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
