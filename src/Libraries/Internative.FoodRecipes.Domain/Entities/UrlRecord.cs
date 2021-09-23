using Internative.FoodRecipes.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Entities
{
    public class UrlRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the slug
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the record is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageId { get; set; }
    }
}
