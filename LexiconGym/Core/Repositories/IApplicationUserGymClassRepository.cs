using LexiconGym.Persistance.Repositories;

namespace LexiconGym.Core.Repositories
{
    public interface IApplicationUserGymClassRepository
    {
        void Add(ApplicationUserGymClassRepository userGymClass); // var sätts userGymClass samman?
        void Remove(ApplicationUserGymClassRepository userGymClass);
    }
}