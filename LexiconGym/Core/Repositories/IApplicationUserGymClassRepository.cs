using LexiconGym.Persistance.Repositories;
using LexiconGym.Core.Models;


namespace LexiconGym.Core.Repositories
{
    public interface IApplicationUserGymClassRepository
    {
        void Add(ApplicationUserGymClass userGymClass); // var sätts userGymClass samman?
        void Remove(ApplicationUserGymClass userGymClass);
    }
}