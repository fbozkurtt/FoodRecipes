using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Areas.Admin.Controllers
{
    [Route("admin/{controller=Admin}/{action=Index}/{id?}")]
    public class AdminController : BaseAdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
