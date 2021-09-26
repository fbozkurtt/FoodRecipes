using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Models
{
    public class LoginModel
    {
        [MinLength(3)]
        [DataType(DataType.Text)]
        [Required]
        public string Username { get; set; }

        [MinLength(5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
