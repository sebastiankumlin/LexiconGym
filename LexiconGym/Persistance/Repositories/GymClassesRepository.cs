using LexiconGym.Core.Models;
using LexiconGym.Core.Repositories;
using LexiconGym.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LexiconGym.Persistance;

namespace LexiconGym.Persistance.Repositories
{
    public class GymClassesRepository : IGymClassesRepository
    {
        private readonly ApplicationDbContext db; //här bor det en databas (-context)

        public GymClassesRepository(ApplicationDbContext db) //här skickas det in en databas-manager-instans.
        {
            this.db = db;
        }

        public void Add(GymClass gymClass) // här kan man operera på databasen m.h.a. EFCore. Repositoryt har sina beteenden som enligt kontrakt med repositoryts Interface. Repositoryts beteenden skapas med EFCore-funktioner.
        {
            //db.Users.Where(u => u.UserName == )
            db.Add(gymClass);
        }
        public async Task<GymClass> GetWithAttendingMembers(int? id)
        {
            return await db.GymClass
                .Include(g => g.AttendingMembers)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        //public async Task<GymClass> GetWithAttendingMembers(int? id)
        //{
        //    return await db.GymClass
        //        .Include(m => m.AttendingMembers) //include populerar propertyt - vi väljer att fylla på med AttendingMembers också
        //        .FirstOrDefaultAsync(g => g.Id == id);
        //}

        public bool Any(int id) //repositoryt ska kunna svara på frågor också. frågefunktionaliteten byggs med EFCore.
        {
           return db.GymClass.Any(g => g.Id == id);
        }

        public async Task<IEnumerable<GymClass>> GetAllAsync() //repositoryt ska kunna arbeta båda vägar
        {
            return await db.GymClass.ToListAsync();
        }

        public async Task<GymClass> GetAsync(int? id)
        {
            return await db.GymClass.FindAsync(id); // repositoryt ska kunna leverera båda samlingar och enskilda entiteter
        }


        

        //public async GymClass GetWithAttendingMembers(int? id) //detta ska levereras till controllern så att vi kan operera på rätt GymClass. Vi måste således ha rätt GymClass - för det använder vi det aktuella Id som vi kommer få genom vyn
        //{
        //    return db.UserGymClass.
        //}

        public void Remove(GymClass gymClass) //repositoryt måste ha den mest grundläggande db-funktionaliteten
        {
            db.GymClass.Remove(gymClass);
        }

       public void Update(GymClass gymClass) //lite hjälp.
        {
            db.GymClass.Update(gymClass);
        }      
    }
}
