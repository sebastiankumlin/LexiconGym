using LexiconGym.Core.Repositories;
using LexiconGym.Core.Models;
using LexiconGym.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexiconGym.Persistance.Repositories;

namespace LexiconGym.Persistance.Repositories
{
    public class ApplicationUserGymClassRepository : IApplicationUserGymClassRepository
    {
        private ApplicationDbContext db;

        public ApplicationUserGymClassRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Add(ApplicationUserGymClass userGymClass)
        {
            db.UserGymClass.Add(userGymClass); //mitt feltänk: db.ApplicationUserGymClass
        }

        public void Remove(ApplicationUserGymClass attending)
        {
            db.UserGymClass.Remove(attending);
        }
    }
}
