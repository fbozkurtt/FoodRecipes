using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/{controller=Admin}/{action=Index}/{id?}")]
    public class BaseAdminController : Controller
    {
    }
}
