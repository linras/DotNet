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
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Dogs.Controllers
{
    //USUWANIE ZDARZEN Z PRZESZLOSCI
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //public EventsController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Events
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Events.Include(d => d.Doge);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Events/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(d => d.Doge)
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Soon
        [AllowAnonymous]
        public async Task<IActionResult> Soon()
        {
            var applicationDbContext = _context.Events.Include(d => d.Doge);
            var events = await GetEvents()
                .ToListAsync();
            IEnumerable<Event> query = events.OrderBy(e => e.When);
            //AUTOMATICALLY REMOVE OLD EVENTS
            foreach (Event item in query)
            {
                if (item.When.CompareTo(DateTime.Now) < 0)
                {
                    //return Json($"Data przeszla: {item.When}");
                    var @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == item.EventId);
                    _context.Events.Remove(@event);
                    await _context.SaveChangesAsync();
                }
                else {
                    break;
                    //return Json($"Data przyszla: {item.When}");
                }
            }
            return View(query);
        }

        private IQueryable<Event> GetEvents()
        {

            return _context.Events
                .Include(e => e.Doge)
                .Include(e => e.User
                );
        }

        // GET: Events/Create
        [Authorize(Roles = "Administrator,User")]
        public IActionResult Create()
        {
            //if (!(await IsUserAdministratorAsync()))
            //{
            //    return View("AccessDenied");
            //}
            ViewData["DogId"] = new SelectList(_context.Dogs, "DogId", "DogId");
            return View();
        }

        

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> Create([Bind("EventId,DogId,When,Description")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                if (_signInManager.IsSignedIn(HttpContext.User))
                {
                    var user = await _userManager.GetUserAsync(User);
                    @event.User = user;
                    _context.Update(@event); //????????????????????
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "DogId", "DogId", @event.DogId);
            return View(@event);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }
            ViewData["DogId"] = new SelectList(_context.Dogs, "DogId", "DogId", @event.DogId);
            return View(@event);
        }

        // POST: Events/Edit/5
        [Authorize(Roles = "Administrator")]
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,DogId,When,Description")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            ViewData["DogId"] = new SelectList(_context.Dogs, "DogId", "DogId", @event.DogId);
            return View(@event);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(d => d.Doge)
                .SingleOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.SingleOrDefaultAsync(m => m.EventId == id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
