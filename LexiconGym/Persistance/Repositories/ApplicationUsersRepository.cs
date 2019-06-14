using LexiconGym.Core.Models;
using LexiconGym.Core.Repositories;
using LexiconGym.Data;
using LexiconGym.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Persistance.Repositories
{
    public class ApplicationUsersRepository : IApplicationUsersRepository
    {
        private ApplicationDbContext db; //här bor det en dbms (EFCore) så att vi kan använda dess beteenden

        public ApplicationUsersRepository(ApplicationDbContext db)
        {
            this.db = db;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public bool Any()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser Find()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetAsync()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
