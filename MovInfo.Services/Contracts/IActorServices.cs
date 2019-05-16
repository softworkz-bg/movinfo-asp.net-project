using MovInfo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovInfo.Services.Contracts
{
    public interface IActorServices
    {
        Task<ICollection<Actor>> FindActorsAsync(string firstName, string ignoredActors);       

        Task<IList<Movie>> GetActorMoviesAsync(long actorId);

        Task<Actor> ShowActorInfoAsync(long actorId);

        Task<Actor> DeleteActorAsync(long actorId, ICollection<string> allowedRoles);

        Task<Actor> EditActorAsync(
            long actorId, 
            string actorFirstName, 
            string actorLastName, 
            string actorBio, 
            string imageName,
            ICollection<string> allowedRoles);

        Task<Actor> AddActorAsync(
           string actorFirstName,
           string actorLastName,
           string actorBio,
           string imageName,
           ICollection<string> allowedRoles);

        Task<IReadOnlyCollection<Actor>> GetActorsByIdAsync();
    }
}
