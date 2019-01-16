using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dogs.Data;
using Dogs.Models;

namespace Dogs.Controllers
{
    public class DogRacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DogRacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DogRaces
        public async Task<IActionResult> Index()
        {
            return View(await _context.DogRaces.ToListAsync());
        }

        // GET: DogRaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogRace = await _context.DogRaces
                .SingleOrDefaultAsync(m => m.DogRaceId == id);
            if (dogRace == null)
            {
                return NotFound();
            }

            return View(dogRace);
        }

        // GET: DogRaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DogRaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogRaceId,Race,Description")] DogRace dogRace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dogRace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dogRace);
        }

        // GET: DogRaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogRace = await _context.DogRaces.SingleOrDefaultAsync(m => m.DogRaceId == id);
            if (dogRace == null)
            {
                return NotFound();
            }
            return View(dogRace);
        }

        // POST: DogRaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DogRaceId,Race,Description")] DogRace dogRace)
        {
            if (id != dogRace.DogRaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dogRace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogRaceExists(dogRace.DogRaceId))
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
            return View(dogRace);
        }

        // GET: DogRaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dogRace = await _context.DogRaces
                .SingleOrDefaultAsync(m => m.DogRaceId == id);
            if (dogRace == null)
            {
                return NotFound();
            }

            return View(dogRace);
        }

        // POST: DogRaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dogRace = await _context.DogRaces.SingleOrDefaultAsync(m => m.DogRaceId == id);
            _context.DogRaces.Remove(dogRace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogRaceExists(int id)
        {
            return _context.DogRaces.Any(e => e.DogRaceId == id);
        }
    }
}
