using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LexiconGym.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Lägg till propertyt Name 1/4
        public string Name { get; set; }
        public ICollection<ApplicationUserGymClass> AttendedClasses { get; set; }
    }
}
