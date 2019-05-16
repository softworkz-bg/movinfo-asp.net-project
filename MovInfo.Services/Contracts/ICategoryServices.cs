using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services.Contracts
{
    public interface ICategoryServices
    {
        Task<ICollection<Category>> FindCategoriesAsync(string categoriesNames, string ignoredCategories);

        Task<Category> AddCategoryAsync(string name, ICollection<string> allowedRoles);

        Task<Category> ShowCategoryInfoAsync(long categoryId);

        Task<IList<Movie>> GetCategoryMoviesAsync(long categoryId);

        Task<Category> EditCategoryAsync(long categoryId, string name, IEnumerable<long> moviesIds, ICollection<string> allowedRoles);

        Task<Category> DeleteCategoryAsync(long categoryId, ICollection<string> allowedRoles);

        Task<IReadOnlyCollection<Category>> GetCategoriesByIdAsync();
    }
}
