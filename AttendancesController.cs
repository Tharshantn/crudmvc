using EmployeeAttendance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAttendance.Controllers
{
    public class AttendancesController : Controller
    {
        
            private readonly AppDbContext _context;

            public AttendancesController(AppDbContext context)
            {
                _context = context;
            }

            // GET: Attendances
            public async Task<IActionResult> Index()
            {
                var appDbContext = _context.Attendances.Include(a => a.Employee);
                return View(await appDbContext.ToListAsync());
            }

            // GET: Attendances/Create
            public IActionResult Create()
            {
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name");
                return View();
            }

        // POST: Attendances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttendanceId,EmployeeId,Date,CheckInTime,CheckOutTime")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(attendance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database Error: {ex.Message}"); // Log any database exception
                    ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", attendance.EmployeeId);
                    return View(attendance);
                }

            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}"); // This helps you see errors
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", attendance.EmployeeId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance == null)
                {
                    return NotFound();
                }
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", attendance.EmployeeId);
                return View(attendance);
            }

            // POST: Attendances/Edit/5
            [HttpPost]
            [ValidateAntiForgeryToken]


            public async Task<IActionResult> Edit(int id, [Bind("AttendanceId,EmployeeId,Date,CheckInTime,CheckOutTime")] Attendance attendance)
            {
                if (id != attendance.AttendanceId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(attendance);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AttendanceExists(attendance.AttendanceId))
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
                ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "Name", attendance.EmployeeId);
                return View(attendance);
            }

            // GET: Attendances/Delete/5
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var attendance = await _context.Attendances
                    .Include(a => a.Employee)
                    .FirstOrDefaultAsync(m => m.AttendanceId == id);
                if (attendance == null)
                {
                    return NotFound();
                }

                return View(attendance);
            }
            // POST: Attendances/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var attendance = await _context.Attendances.FindAsync(id);
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool AttendanceExists(int id)
            {
                return _context.Attendances.Any(e => e.AttendanceId == id);
            }
        }
    }


