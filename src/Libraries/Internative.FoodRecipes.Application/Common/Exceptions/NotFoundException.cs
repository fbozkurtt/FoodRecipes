using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base()
        {

        }

        public NotFoundException(string message) : base(message)
        {

        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public NotFoundException(string name, object key, object value)
            : base($"Entity {name} with the {key} of '{value}' was not found.")
        {

        }
    }
}
