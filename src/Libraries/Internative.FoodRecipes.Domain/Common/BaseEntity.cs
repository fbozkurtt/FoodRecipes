using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Common
{
    public abstract partial class BaseEntity
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public int? LastModifiedBy { get; set; }
    }
}
