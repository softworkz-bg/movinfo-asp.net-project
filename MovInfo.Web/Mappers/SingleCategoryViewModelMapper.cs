using Microsoft.Extensions.Configuration;
using MovInfo.Models;
using MovInfo.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.Mappers
{
    public class SingleCategoryViewModelMapper : IViewModelMapper<Category, SingleCategoryViewModel>
    {
        public SingleCategoryViewModel MapFrom(Category entity)
        => new SingleCategoryViewModel
        {
            Id = entity.Id,
            Title = entity.Title
        };
    }
}
