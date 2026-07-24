using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    public class LeaveTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Leave Type List
        // ==========================
        public async Task<IActionResult> Index()
        {
            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            return View(leaveTypes);
        }

        // ==========================
        // Create Leave Type (GET)
        // ==========================
        public IActionResult Create()
        {
            return View();
        }

        // ==========================
        // Create Leave Type (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveType leaveType)
        {
            if (ModelState.IsValid)
            {
                _context.LeaveTypes.Add(leaveType);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Leave type added successfully.";

                return RedirectToAction(nameof(Index));
            }

            return View(leaveType);
        }

        // ==========================
        // Leave Type Details
        // ==========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(x => x.LeaveTypeId == id);

            if (leaveType == null)
                return NotFound();

            return View(leaveType);
        }

        // ==========================
        // Edit Leave Type (GET)
        // ==========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
                return NotFound();

            return View(leaveType);
        }

        // ==========================
        // Edit Leave Type (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveType leaveType)
        {
            if (id != leaveType.LeaveTypeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveType);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Leave type updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(leaveType);
        }

        // ==========================
        // Delete Leave Type (GET)
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(x => x.LeaveTypeId == id);

            if (leaveType == null)
                return NotFound();

            return View(leaveType);
        }

        // ==========================
        // Delete Leave Type (POST)
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Leave type deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // AJAX Delete
        // ==========================
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Leave type not found."
                });
            }

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Leave type deleted successfully."
            });
        }
    }
}