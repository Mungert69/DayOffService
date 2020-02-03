using DayOff.Data;
using DayOff.Models;
using DaysOff.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Controllers
{
    public class HolidaysController : Controller
    {
        private readonly DayOffContext _context;

        public HolidaysController(DayOffContext context)
        {
            _context = context;
        }

        public List<SelectListItem> HolTypeSelectList => Enum.GetValues(typeof(HolTypes)).Cast<HolTypes>().Select(v => new SelectListItem
        {

            Text = v.ToString(),
            Value = ((int)v).ToString()
        }).ToList();

        public List<SelectListItem> DurationSelectList => Enum.GetValues(typeof(Durations)).Cast<Durations>().Select(v => new SelectListItem
        {

            Text = v.ToString(),
            Value = ((int)v).ToString()
        }).ToList();

        // GET: Holidays
        public async Task<IActionResult> Index()
        {
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Holiday, User> dayOffContext = _context.Holidays.Include(h => h.User);
            return View(await dayOffContext.ToListAsync());
        }

        // GET: Holidays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Holiday holiday = await _context.Holidays
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.HolidayID == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // GET: Holidays/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "FirstName");
            ViewData["HolTypes"] = HolTypeSelectList;
            ViewData["Durations"] = DurationSelectList;
            return View();
        }

        // POST: Holidays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HolidayID,UserID,HolType,Duration,HolDate")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holiday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "FirstName", holiday.UserID);
            return View(holiday);
        }

        // GET: Holidays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Holiday holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "FirstName", holiday.UserID);
            ViewData["HolTypes"] = HolTypeSelectList;
            ViewData["Durations"] = DurationSelectList;
            return View(holiday);
        }

        // POST: Holidays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HolidayID,UserID,HolType,Duration,HolDate")] Holiday holiday)
        {
            if (id != holiday.HolidayID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(holiday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayExists(holiday.HolidayID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "ID", "FirstName", holiday.UserID);
            return View(holiday);
        }

        // GET: Holidays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Holiday holiday = await _context.Holidays
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.HolidayID == id);
            if (holiday == null)
            {
                return NotFound();
            }

            return View(holiday);
        }

        // POST: Holidays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Holiday holiday = await _context.Holidays.FindAsync(id);
            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HolidayExists(int id)
        {
            return _context.Holidays.Any(e => e.HolidayID == id);
        }
    }
}
