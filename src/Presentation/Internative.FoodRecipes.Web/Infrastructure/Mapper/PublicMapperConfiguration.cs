using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Internative.FoodRecipes.Application.Common.Mapper;
using Internative.FoodRecipes.Domain.Entities;
using Internative.FoodRecipes.Web.Models;

namespace Internative.FoodRecipes.Web.Infrastructure.Mapper
{
    public class PublicMapperConfiguration : Profile, IOrderedMapperProfile
    {
        #region Ctor

        public PublicMapperConfiguration()
        {
            CreateMaps();
        }

        #endregion

        #region Utilites

        protected virtual void CreateMaps()
        {
            CreateMap<Recipe, RecipeModel>()
                .ForMember(_=>_.Ingredients, options => options.Ignore());
            CreateMap<RecipeModel, Recipe>()
                .ForMember(_ => _.Ingredients, options => options.Ignore());
        }

        #endregion

        #region Properties

        public int Order => 0;

        #endregion
    }
}
