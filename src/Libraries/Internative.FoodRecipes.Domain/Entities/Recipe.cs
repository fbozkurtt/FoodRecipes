using Internative.FoodRecipes.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Entities
{
    public partial class Recipe : BaseEntity
    {
        public string Title { get; set; }
        public string Description  { get; set; }
        public int? CookingTime { get; set; }
        public int? PreparingTime { get; set; }
    }
}
