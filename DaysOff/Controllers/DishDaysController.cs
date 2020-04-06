using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DayOff.Data;
using DaysOff.Models;
using DayOff.Models;

namespace DaysOff.Controllers
{
    public class DishDaysController : Controller
    {
        private readonly DayOffContext _context;

        public DishDaysController(DayOffContext context)
        {
            _context = context;
        }

        // GET: DishDays
        public async Task<IActionResult> Index()
        {
            var dayOffContext = _context.DishDays.Include(d => d.User);
            return View(await dayOffContext.ToListAsync());
        }

        // GET: DishDays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishDay = await _context.DishDays
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dishDay == null)
            {
                return NotFound();
            }

            return View(dishDay);
        }

        // GET: DishDays/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "ID");
            return View();
        }

        // POST: DishDays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DishID,UserID,DishDate")] DishDay dishDay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishDay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "ID", dishDay.UserID);
            return View(dishDay);
        }

        // GET: DishDays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishDay = await _context.DishDays.FindAsync(id);
            if (dishDay == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "ID", dishDay.UserID);
            return View(dishDay);
        }

        // POST: DishDays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DishID,UserID,DishDate")] DishDay dishDay)
        {
            if (id != dishDay.DishID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishDay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishDayExists(dishDay.DishID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "ID", dishDay.UserID);
            return View(dishDay);
        }

        // GET: DishDays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dishDay = await _context.DishDays
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.DishID == id);
            if (dishDay == null)
            {
                return NotFound();
            }

            return View(dishDay);
        }

        // POST: DishDays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dishDay = await _context.DishDays.FindAsync(id);
            _context.DishDays.Remove(dishDay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishDayExists(int id)
        {
            return _context.DishDays.Any(e => e.DishID == id);
        }
    }
}
