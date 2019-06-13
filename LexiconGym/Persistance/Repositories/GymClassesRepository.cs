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

        public void Add(GymClass gymClass)
        {
            db.Add(gymClass);
        }

        public bool Any(int id)
        {
           return db.GymClass.Any(g => g.Id == id);
        }

        public async Task<IEnumerable<GymClass>> GetAllAsync()
        {
            return await db.GymClass.ToListAsync();
        }

        public async Task<GymClass> GetAsync(int? id)
        {
            return await db.GymClass.FindAsync(id);
        }

        public void Remove(GymClass gymClass)
        {
            db.GymClass.Remove(gymClass);
        }

        public void Update(GymClass gymClass)
        {
            db.GymClass.Update(gymClass);
        }
    }
}
