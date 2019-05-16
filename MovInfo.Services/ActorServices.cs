using Microsoft.EntityFrameworkCore;
using MovInfo.Data;
using MovInfo.Models;
using MovInfo.Services.Contracts;
using MovInfo.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Services
{
    public class ActorServices : IActorServices
    {
        private readonly MovInfoContext context;
        private readonly IBusinessLogicValidator businessLogicValidator;

        public ActorServices(MovInfoContext context, IBusinessLogicValidator businessLogicValidator)
        {
            this.context = context;
            this.businessLogicValidator = businessLogicValidator;
        }

        public async Task<ICollection<Actor>> FindActorsAsync(string firstName, string ignoredActors)
        {
            IQueryable<Actor> SearchList;

            if (!string.IsNullOrEmpty(ignoredActors))
            {
                var ignoreList = ignoredActors.Split(',').Select(m => long.Parse(m)).ToArray();
                SearchList = context.Actors.Where(a => !ignoreList.Contains(a.Id)).Select(x => x);
            }
            else
            {
                SearchList = from m in context.Actors
                             select m;
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                SearchList = SearchList.Where(s => s.FirstName.Contains(firstName));
            }

            return await SearchList.ToListAsync();
        }

        public async Task<Actor> AddActorAsync(string actorFirstName, string actorLastName, string actorBio, string imageName, ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsActorAlreadyPresent(context, actorFirstName, actorLastName);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var actorToAdd = new Actor
            {
                FirstName = actorFirstName,
                LastName = actorLastName,
                Bio = actorBio,
                ProfileImageName = imageName
            };

            await context.Actors.AddAsync(actorToAdd);

            await context.SaveChangesAsync();

            return actorToAdd;
        }

        public async Task<IList<Movie>> GetActorMoviesAsync(long actorId)
        {
            var actorToFind = await context.Actors.FindAsync(actorId);

            businessLogicValidator.IsEntityFound(actorToFind, BusinessLogicValidatorMessages.NoSuchActor);

            var allActorMovies = await context.Movies.Where(m => m.MoviesActors.Any(ma => ma.ActorId == actorId)).ToListAsync();

            return allActorMovies;
        }

        public async Task<Actor> ShowActorInfoAsync(long actorId)
        {
            var actorToFind = await context.Actors.FindAsync(actorId);

            businessLogicValidator.IsEntityFound(actorToFind, BusinessLogicValidatorMessages.NoSuchActor);

            var resultActor = await context.Actors.Include(a => a.MoviesActors).FirstOrDefaultAsync(a => a.Id == actorId);

            return resultActor;
        }

        public async Task<Actor> EditActorAsync(long actorId, string actorFirstName, string actorLastName, string actorBio, string imageName, ICollection<string> allowedRoles)
        {
            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var actorToEdit = await context.Actors.FindAsync(actorId);

            actorToEdit.FirstName = actorFirstName;
            actorToEdit.LastName = actorLastName;
            actorToEdit.Bio = actorBio;
            actorToEdit.ProfileImageName = imageName;

            await context.SaveChangesAsync();

            return actorToEdit;
        }

        public async Task<Actor> DeleteActorAsync(long actorId, ICollection<string> allowedRoles)
        {
            businessLogicValidator.DoesActorExist(context, actorId);

            businessLogicValidator.IsUserInRoleForService(allowedRoles, BusinessLogicValidatorMessages.NotAuthorized);

            var actorToDelete = await context.Actors.FindAsync(actorId);

            context.Actors.Remove(actorToDelete);

            await context.SaveChangesAsync();

            return actorToDelete;
        }

        public async Task<IReadOnlyCollection<Actor>> GetActorsByIdAsync()
        {
            var actors = await context.Actors
                           .OrderByDescending(a => a.Id)
                           .ToListAsync();

            return actors;
        }
    }
}