using System.Collections.Generic;
using System.Threading.Tasks;
using LexiconGym.Core.Models;

namespace LexiconGym.Core.Repositories
{
    public interface IGymClassesRepository
    {
        Task<IEnumerable<GymClass>> GetAllAsync();
        Task<GymClass> GetAsync(int? id);
        void Add(GymClass gymClass);
        bool Any(int id);
        void Update(GymClass gymClass);
        void Remove(GymClass gymClass);
    }
}