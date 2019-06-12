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

namespace LexiconGym.Controllers
{
    public class GymClassesController : Controller
    {
        private readonly IGymClassesRepository repository;

        public GymClassesController(IGymClassesRepository repository)
        {
            this.repository = repository;
        }

        // GET: GymClasses
        public async Task<IActionResult> Index()
        {
            return View(await repository.GetAllAsync());
        }

        //// GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await repository.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        //// GET: GymClasses/Create
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
                repository.Add(gymClass);
               // await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var gymClass = await _context.GymClass.FindAsync(id);
        //    if (gymClass == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(gymClass);
        //}

        //// POST: GymClasses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartTime,Duration,Description")] GymClass gymClass)
        //{
        //    if (id != gymClass.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(gymClass);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!GymClassExists(gymClass.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(gymClass);
        //}

        //// GET: GymClasses/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var gymClass = await _context.GymClass
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (gymClass == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(gymClass);
        //}

        //// POST: GymClasses/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var gymClass = await _context.GymClass.FindAsync(id);
        //    _context.GymClass.Remove(gymClass);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool GymClassExists(int id)
        //{
        //    return _context.GymClass.Any(e => e.Id == id);
        //}
    }
}
