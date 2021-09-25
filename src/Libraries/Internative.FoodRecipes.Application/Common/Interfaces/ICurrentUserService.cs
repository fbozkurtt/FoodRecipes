using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        public int UserId { get; }

        public string Username { get; }
    }
}
