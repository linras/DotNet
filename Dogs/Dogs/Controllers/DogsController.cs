using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dogs.Data;
using Dogs.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dogs.Controllers
{
    public class DogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dogs
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dogs.Include(d => d.Race);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dogs/Details/5
        [AllowAnonymous]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Race)
                .SingleOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }
            int numberOfEvents = 0;
            List<Event> events = _context.Events.ToList();
            List<Dog> dogs = _context.Dogs.ToList();
            //LINQ JOIN
            var eventsQuery =
                    from e in events
                    join d in dogs on e.Doge.DogId equals d.DogId into eq
                    select new { Key = e.Doge.DogId, Items = eq };
            foreach (var item in eventsQuery) {
                if (item.Key == id)
                    numberOfEvents++;
            }
            //numberOfEvents = eventsQuery.Count();
            DogViewModel dogViewModel = new DogViewModel();
            dogViewModel.Name = dog.Name;
            dogViewModel.DateOfBirth = dog.DateOfBirth;
            dogViewModel.Description = dog.Description;
            dogViewModel.DogId = dog.DogId;
            dogViewModel.Race = dog.Race;
            dogViewModel.HereSince = dog.HereSince;
            dogViewModel.NumberOfEvents = numberOfEvents;
            return View(dogViewModel);
        }

        // GET: Dogs/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["DogRaceId"] = new SelectList(_context.DogRaces, "DogRaceId", "DogRaceId");
            return View();
        }

        // POST: Dogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DogId,Name,DateOfBirth,HereSince,Description,DogRaceId")] DogViewModel dog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DogRaceId"] = new SelectList(_context.DogRaces, "DogRaceId", "DogRaceId", dog.DogRaceId);
            return View(dog);
        }

        // GET: Dogs/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs.SingleOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }
            ViewData["DogRaceId"] = new SelectList(_context.DogRaces, "DogRaceId", "DogRaceId", dog.DogRaceId);
            return View(dog);
        }

        // POST: Dogs/Edit/5
        [Authorize(Roles = "Administrator")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DogId,Name,DateOfBirth,HereSince,Description,DogRaceId")] Dog dog)
        {
            if (id != dog.DogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DogExists(dog.DogId))
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
            ViewData["DogRaceId"] = new SelectList(_context.DogRaces, "DogRaceId", "DogRaceId", dog.DogRaceId);
            return View(dog);
        }

        // GET: Dogs/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dog = await _context.Dogs
                .Include(d => d.Race)
                .SingleOrDefaultAsync(m => m.DogId == id);
            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dog = await _context.Dogs.SingleOrDefaultAsync(m => m.DogId == id);
            _context.Dogs.Remove(dog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DogExists(int id)
        {
            return _context.Dogs.Any(e => e.DogId == id);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> VerifyDate(DateTime birth, DateTime hereSince)
        {
            //if empty take current date
            if (hereSince.Equals(null))
            {
                hereSince = DateTime.Now;
            }
            //birth date can be unknown
            if (birth.Equals(null))
                return Json(true);
            
            if(birth.CompareTo(hereSince)==0)
                return Json($"Impossible for dog to be here faster then it's birth. {@birth} {@hereSince}");
            return Json(true);
        }
    }
}
