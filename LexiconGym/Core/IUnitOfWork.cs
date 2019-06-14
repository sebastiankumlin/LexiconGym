using System.Threading.Tasks;
using LexiconGym.Core.Repositories;

namespace LexiconGym.Core
{
    public interface IUnitOfWork
    {
        IGymClassesRepository GymClasses { get; set; }
        IApplicationUserGymClassRepository UserGymClass { get; set; }

        Task CompleteAsync();
    }
}