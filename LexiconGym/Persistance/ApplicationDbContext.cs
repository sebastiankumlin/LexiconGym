using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LexiconGym.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace LexiconGym.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string> //verkar bara tjäna Identify-funktionaliteten
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) //var är tabellerna över Users och deras Roles?
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserGymClass>() //bygg en kopplingstabell
                .HasKey(k => new
                {
                    k.ApplicationUserId,
                    k.GymClassId
                });
        }
        public DbSet<GymClass> GymClass { get; set; } //här har vi en tabell därför att det är ju bra om dbms (context) känner till att det finns
        public DbSet<ApplicationUserGymClass> UserGymClass { get; set; } //nu vet dbms att denna tabell/relation finns
    }
}
