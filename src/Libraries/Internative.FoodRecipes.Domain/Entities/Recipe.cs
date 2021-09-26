using Internative.FoodRecipes.Domain.Common;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Entities
{
    public partial class Recipe : BaseEntity
    {
        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("instructions")]
        public IList<string> Instructions  { get; set; }

        [BsonElement("ingredients")]
        public IList<Ingredient> Ingredients { get; set; }

        [BsonElement("cookingTime")]
        public int? CookingTime { get; set; }

        [BsonElement("preparingTime")]
        public int? PreparingTime { get; set; }

        [BsonElement("totalViews")]
        public int TotalViews { get; set; }

        [BsonElement("pictureIds")]
        public int[] PictureIds { get; set; }

        [BsonElement("viewCount")]
        public int ViewCount { get; set; }

        #region Nested Classes

        public class Ingredient
        {
            public string Name { get; set; }
            public string Amount { get; set; }
            public string Unit { get; set; }
        }

        #endregion
    }
}
