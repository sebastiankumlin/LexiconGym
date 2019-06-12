using LexiconGym.Core.Models;
using LexiconGym.Core.Repositories;
using LexiconGym.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Persistance.Repositories
{
    public class GymClassesRepository : IGymClassesRepository
    {
        private readonly ApplicationDbContext db;

        public GymClassesRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<GymClass>> GetAllAsync()
        {
            return await db.GymClass.ToListAsync();
        }
    }
}
