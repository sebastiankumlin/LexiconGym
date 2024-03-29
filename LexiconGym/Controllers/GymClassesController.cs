﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LexiconGym.Core.Models;
using LexiconGym.Data;
using LexiconGym.Core.Repositories;
using LexiconGym.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LexiconGym.Persistance.Repositories;

namespace LexiconGym.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager; //Min slav!

        public GymClassesController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Member")]
        public async Task<IActionResult> BookingToggle(int? id) //id måste vara definierat någonstans för att kunna skickas in

            //vi måste läsa in hela kedjan för GymClass - ApplicationUserGymClasses - ApplicationUser så att vi kan göra flera olika analyser av gymklassens besökande medlemmar och medlemmars inbokade gymklasser.
        {
            //matchning måste ske mellan användare och gymlektioner, antingen genom navprop på user, kopplingstabell eller genom jämförelse på alla gymlektioner
            //hur ska en member kunna boka in sig på gymlektioner genom UI? Det finns ingen sådan funktionalitet just nu.

            if (id == null) return NotFound();

            //FYI UserName på Users är en inbyggd metod, den är inte så smart och den förbjuder sannolikt dubletter.
            //DbSet Users är en ärvd klass och syns inte i DbContext
            
            var currentUser = await userManager.GetUserAsync(User); //id för en user sitter på IdentityUser och som är base class till ApplicationUser. ApplicationUser innehåller därför inte Id. //Du ska ha en application User (lägga till IdentityUser) - så man får ta ett nytt kontext - Microsoft visar inte svagheterna med det och tvärtom om man börjar från andra hållet. // Annars får man börja med IdentityUser //Först identityUser - med UserName och lock out - om man vill extenda den - då måste vi extenda den med name och navprop - När vi scaffoldat det så utgår maskinen från att IdentituUser så då måste vi ändra alla platser där IdentityUser scaffoldades up. //Systemet gör båda ----------------- Man måste ha två kontext för att kunna fixa scaffoldingproblemet (konfigureringen kan upprättas automatiskt) 
            //Services får man lägga till i StartUp efter exeempelvis en AddScoped
            //
            //GymClass innehåller en ICollection för ApplicationUserGymClass.. Alla dom har GymClassId - GymClass innehåller en ApplicationUserId - så vi kan titta på GymClass-klass för att 

            var currentGymClass = await unitOfWork.GymClasses.GetWithAttendingMembers(id); //Skapa ny metod, den förra hette GetAsync, vi får //I GymClass kallar vi det för attending members - det är vår navigation property och behöver kopplingstabellen för att fungera
            //Nu vill vi titta på om någon av GymPassNycklarna innehåller User GUID
            //Hämta aktuellt gympass läs dör vi även hämtar navigationproperty -- GetWithAttendingMembers ligger på repositoryt eftersom vi använder UnitOfWork. 

            if (currentGymClass == null) return NotFound();

            //är den inloggande användaren bokad på passet? notera språket - eftersom det alltid är en användare som går den här vägen - så är det först här som inloggadhet prövas.
            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == currentUser.Id //ApplicationUserId sitter på ApplicationUserGymClass.cs //Kopplingstabellen svarar på frågan "finns det en koppling mellan instanserna?".
                && u.GymClassId == id); //det ska ju vara en nyckel som vi tar fram så det ska vara unikt - queryn gör så att så är fallet                

            if(attending == null)
            {
                var book = new ApplicationUserGymClass //upprätta en instans av klassen book.
                {
                    ApplicationUserId = currentUser.Id, //den har id för attendee
                    GymClassId = currentGymClass.Id //den har id för gym lektionen
                };

                unitOfWork.UserGymClass.Add(book); //unitOfWork innehåller tjänster referenser till tjänster?
                await unitOfWork.CompleteAsync(); //null har vi skapat en ny kompositnyckel -eftersom de ska sitta ihop                
            } else
            {
                unitOfWork.UserGymClass.Remove(attending); //vi gik ju till vår attendingmembersoch selejterades
                await unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(Index));
            //var allClasses = await unitOfWork.GymClasses.GetAllAsync();



            
        }

        [AllowAnonymous]
        // GET: GymClasses
        public async Task<IActionResult> Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return View(await unitOfWork.GymClasses.GetAllWithUsersAsync());
            }
            if (User.IsInRole("Member"))
            {
                var userId = userManager.GetUserId(User); //Usern finns tydligen överallt
                var model = await unitOfWork.GymClasses.GetAllWithUsersAsync();
                return View(model);
            }

            //return NotAuthorized

            return View(await unitOfWork.GymClasses.GetAllWithUsersAsync());
        }

        //// GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        //// GET: GymClasses/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        //// POST: GymClasses/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.GymClasses.Add(gymClass);
                await unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        //// POST: GymClasses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.GymClasses.Update(gymClass);
                    await unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        //// POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await unitOfWork.GymClasses.GetAsync(id);
            unitOfWork.GymClasses.Remove(gymClass);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            //Ny metod på Repositoriet
             return unitOfWork.GymClasses.Any(id);
        }
    }
}
