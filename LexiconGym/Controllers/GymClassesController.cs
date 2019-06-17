using System;
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
        {
            //matchning måste ske mellan användare och gymlektioner, antingen genom navprop på user, kopplingstabell eller genom jämförelse på alla gymlektioner
            //hur ska en member kunna boka in sig på gymlektioner genom UI? Det finns ingen sådan funktionalitet just nu.

            if (id == null) return NotFound();
            
            var currentUser = await userManager.GetUserAsync(User);
            //GymClass innehåller en ICollection för ApplicationUserGymClass.. Alla dom har GymClassId - GymClass innehåller en ApplicationUserId - så vi kan titta på GymClass-klass för att 
            var currentGymClass = await unitOfWork.GymClasses.GetWithAttendingMembers(id); //Skapa ny metod, den förra hette GetAsync, vi får //I GymClass kallar vi det för attending members - det är vår navigation property och behöver kopplingstabellen för att fungera
            //Nu vill vi titta på om någon av GymPassNycklarna innehåller User GUID

            if (currentGymClass == null) return NotFound();

            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == currentUser.Id
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
            return View(await unitOfWork.GymClasses.GetAllAsync());
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
