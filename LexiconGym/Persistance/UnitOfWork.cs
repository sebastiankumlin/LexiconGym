using LexiconGym.Core;
using LexiconGym.Core.Repositories;
using LexiconGym.Data;
using LexiconGym.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;

        public IGymClassesRepository GymClasses { get; set; }

        public IApplicationUsersRepository ApplicationUsers { get; set; } //den här tycker jag var bra

        public IApplicationUserGymClassRepository UserGymClass { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            this.db = db;
            GymClasses = new GymClassesRepository(db); //här lägger jag till en ny GymClassRepository med en dbms
            ApplicationUsers = new ApplicationUsersRepository(db);
            UserGymClass = new ApplicationUserGymClassRepository(db);
        }

        public async Task CompleteAsync() //Arbeta på databasen. Använd dbms för att implementera funktionaliteten, som vanligt.
        {
            await db.SaveChangesAsync();
        }
    }
}
