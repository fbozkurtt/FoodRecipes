using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Areas.Admin.Controllers
{
    public class UserController : BaseAdminController
    {
        public async Task<IActionResult> List()
        {
            return View();
        }
    }
}
