using Microsoft.EntityFrameworkCore;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly MovInfoContext context;
        private readonly IBusinessLogicValidator businessLogicValidator;

        public CategoryServices(MovInfoContext context, IBusinessLogicValidator businessLogicValidator)
        {
            this.context = context;
            this.businessLogicValidator = businessLogicValidator;
        }

        public async Task<ICollection<Category>> FindCategoriesAsync(string categoriesNames, string ignoredCategories)
        {
            IQueryable<Category> SearchList;

            if (!string.IsNullOrEmpty(ignoredCategories))
            {
                var ignoreList = ignoredCategories.Split(',').Select(m => long.Parse(m)).ToArray();
                SearchList = context.Categories.Where(a => !ignoreList.Contains(a.Id)).Select(x => x);
            }
            else
            {
                SearchList = from m in context.Categories
                             select m;
            }

            if (!string.IsNullOrEmpty(categoriesNames))
            {
                SearchList = SearchList.Where(s => s.Title.Contains(categoriesNames));
            }

            return await SearchList.ToListAsync();
        }

        public async Task<Category> AddCategoryAsync(string name, ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsCategoryAlreadyPresent(context, name);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var categoryToAdd = new Category
            {
                Title = name
            };

            await context.Categories.AddAsync(categoryToAdd);

            await context.SaveChangesAsync();

            return categoryToAdd;
        }

        public async Task<Category> ShowCategoryInfoAsync(long categoryId)
        {
            var categoryToFind = await context.Categories.FindAsync(categoryId);

            businessLogicValidator.IsEntityFound(categoryToFind, BusinessLogicValidatorMessages.NoSuchCategory);

            var resultCategory = await context.Categories.Include(a => a.MovieCategories).FirstOrDefaultAsync(a => a.Id == categoryId);

            return resultCategory;
        }

        public async Task<IList<Movie>> GetCategoryMoviesAsync(long categoryId)
        {
            var categoryToFind = await context.Categories.FindAsync(categoryId);

            businessLogicValidator.IsEntityFound(categoryToFind, BusinessLogicValidatorMessages.NoSuchCategory);
                
            var allCategoryMovies =  await context.Movies.Where(m => m.MoviesCategories.Any(mc => mc.CategoryId == categoryId)).ToListAsync();

            return allCategoryMovies;
        }

        public async Task<Category> EditCategoryAsync(long categoryId, string name, IEnumerable<long> moviesIds, ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var categoryToEdit = await context.Categories.Include(a => a.MovieCategories).FirstOrDefaultAsync(a => a.Id == categoryId);

            categoryToEdit.Title = name;

            var categoryMoviesList = moviesIds.Select(movie => new MoviesCategories() { MovieId = movie }).ToList();

            context.TryUpdateManyToMany(categoryToEdit.MovieCategories, categoryMoviesList.Select(x => new MoviesCategories
            {
                MovieId = x.MovieId,
                CategoryId = categoryId
            }), x => x.MovieId);

            await context.SaveChangesAsync();

            return categoryToEdit;
        }

        public async Task<Category> DeleteCategoryAsync(long categoryId, ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesCategoryExist(context, categoryId);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var categoryToDelete = await context.Categories.FindAsync(categoryId);

            context.Categories.Remove(categoryToDelete);

            await context.SaveChangesAsync();

            return categoryToDelete;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoriesByIdAsync()
        {
            var categories = await context.Categories
                           .OrderByDescending(c => c.Id)
                           .ToListAsync();

            return categories;
        }
    }
}
