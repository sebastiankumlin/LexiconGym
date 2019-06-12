using LexiconGym.Core.Repositories;
using LexiconGym.Data;
using LexiconGym.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Persistance
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext db;
        public IGymClassesRepository GymClasses { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            GymClasses = new GymClassesRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}
