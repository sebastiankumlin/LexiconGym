using System.Collections.Generic;
using System.Threading.Tasks;
using LexiconGym.Core.Models;
using LexiconGym.Persistance.Repositories;

namespace LexiconGym.Core.Repositories
{
    public interface IGymClassesRepository
    {
        Task<IEnumerable<GymClass>> GetAllAsync();
        Task<GymClass> GetAsync(int? id);
        void Add(GymClass gymClass);
        bool Any(int id);
        //void Update(GymClass gymClass);
        void Remove(GymClass gymClass);
        Task<GymClass> GetWithAttendingMembers(int? id);
        void Update(GymClass gymClass);

    }
}