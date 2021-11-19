using System;
using System.Collections.Generic;

namespace FoodRecipes.Data.Mapping
{
    /// <summary>
    /// Base instance of backward compatibility of table naming
    /// </summary>
    public partial class BaseNameCompatibility : INameCompatibility
    {
        public Dictionary<Type, string> TableNames => new Dictionary<Type, string>
        {
            //{ typeof(NewTableName), "OldTableName" },
        };

        public Dictionary<(Type, string), string> ColumnName => new Dictionary<(Type, string), string>
        {
            //{ (typeof(Entity), "NewColumnName"), "OldColumnName" },
        };
    }
}