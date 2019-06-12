using System.Collections.Generic;
using System.Threading.Tasks;
using LexiconGym.Core.Models;

namespace LexiconGym.Core.Repositories
{
    public interface IGymClassesRepository
    {
        Task<IEnumerable<GymClass>> GetAllAsync();
    }
}