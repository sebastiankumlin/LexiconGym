using LexiconGym.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Core.Repositories
{
    public interface IApplicationUsersRepository
    {
        void Add();
        void Remove();
        bool Any(); //shortcut

        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<ApplicationUser> GetAsync();

        ApplicationUser Find(); //predicate

        void Update(); //in-memory objects cannot be updated

        // IF YOU WANT to do something with the database at all, you must use an UnitOfWork
        // a UoW keeps track of the changes made to objects and later writes these changes to database
        // kom ihåg att det är skillad på att använda databasen och att arbeta på databasen. Det första är read det andra är write. Endast write är ett arbete.
    }
}
