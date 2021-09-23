using Internative.FoodRecipes.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Domain.Entities
{
    /// <summary>
    /// Represents a picture binary data
    /// </summary>
    public partial class PictureBinary : BaseEntity
    {
        /// <summary>
        /// Gets or sets the picture binary
        /// </summary>
        public byte[] BinaryData { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }
    }
}
