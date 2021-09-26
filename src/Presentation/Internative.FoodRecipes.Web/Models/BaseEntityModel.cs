using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Web.Models
{
    public partial record BaseEntityModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int? LastModifiedBy { get; set; }
    }
}
