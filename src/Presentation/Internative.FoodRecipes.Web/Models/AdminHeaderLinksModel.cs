using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Models
{
    public class AdminHeaderLinksModel
    {
        public bool DisplayAdminLink { get; set; } = true;
        public string EditPageUrl { get; set; }
    }
}
