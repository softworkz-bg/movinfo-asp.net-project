using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services.Contracts
{
    public interface IReviewServices
    {
        Task<Review> AddReviewAsync(string userName, string userId, long movieId, double reviewRating, string reviewText, ICollection<string> allowedRoles);

        Task<Review> GetReviewAsync(long reviewId);

        Task<Review> UpdateReviewAsync(long reviewId, string userName, string userId, long movieId, double reviewRating, double oldRating, string reviewText, ICollection<string> allowedRoles);

        Task<Review> DeleteReviewAsync(long reviewId, ICollection<string> allowedRoles);
    }
}
