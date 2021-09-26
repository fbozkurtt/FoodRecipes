using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Internative.FoodRecipes.Domain.Common
{
    public abstract partial class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }


        [BsonElement("created")]
        public DateTime Created { get; set; }


        [BsonElement("createdBy")]

        public int? CreatedBy { get; set; }


        [BsonElement("lastModified")]

        public DateTime? LastModified { get; set; }


        [BsonElement("lastModifiedBy")]

        public int? LastModifiedBy { get; set; }
    }
}
