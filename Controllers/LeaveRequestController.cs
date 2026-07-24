using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Leave Request List
        // ==========================
        public async Task<IActionResult> Index()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .OrderByDescending(l => l.AppliedDate)
                .ToListAsync();

            return View(leaveRequests);
        }

        // ==========================
        // Create (GET)
        // ==========================
        public IActionResult Create()
        {
            LoadDropdowns();

            return View();
        }

        // ==========================
        // Create (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest leaveRequest)
        {
            if (leaveRequest.StartDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("", "Start Date cannot be in the past.");
            }

            if (leaveRequest.EndDate < leaveRequest.StartDate)
            {
                ModelState.AddModelError("", "End Date cannot be before Start Date.");
            }

            leaveRequest.TotalDays =
                (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            leaveRequest.Status = "Pending";
            leaveRequest.AppliedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.LeaveRequests.Add(leaveRequest);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Leave request submitted successfully.";

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();

            return View(leaveRequest);
        }

        // ==========================
        // Details
        // ==========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.LeaveRequestId == id);

            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }

        // ==========================
        // Edit (GET)
        // ==========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveRequest =
                await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
                return NotFound();

            LoadDropdowns();

            return View(leaveRequest);
        }

        // ==========================
        // Edit (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequest leaveRequest)
        {
            if (id != leaveRequest.LeaveRequestId)
                return NotFound();

            leaveRequest.TotalDays =
                (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            if (ModelState.IsValid)
            {
                _context.Update(leaveRequest);

                await _context.SaveChangesAsync();

                TempData["Success"] = "Leave request updated successfully.";

                return RedirectToAction(nameof(Index));
            }

            LoadDropdowns();

            return View(leaveRequest);
        }

        // ==========================
        // Delete (GET)
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var leaveRequest = await _context.LeaveRequests
                .Include(l => l.Employee)
                .Include(l => l.LeaveType)
                .FirstOrDefaultAsync(l => l.LeaveRequestId == id);

            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }

        // ==========================
        // Delete (POST)
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest =
                await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);

                await _context.SaveChangesAsync();

                TempData["Success"] =
                    "Leave request deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // AJAX Delete
        // ==========================
        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var leaveRequest =
                await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Leave request not found."
                });
            }

            _context.LeaveRequests.Remove(leaveRequest);

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Leave request deleted successfully."
            });
        }

        // ==========================
        // Approve Leave Request
        // ==========================
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
            {
                TempData["Error"] = "Leave request not found.";
                return RedirectToAction(nameof(Index));
            }

            leaveRequest.Status = "Approved";
            leaveRequest.ApprovedDate = DateTime.Now;
            leaveRequest.ApprovedBy = "Admin";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Leave request approved successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Reject Leave Request
        // ==========================
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null)
            {
                TempData["Error"] = "Leave request not found.";
                return RedirectToAction(nameof(Index));
            }

            leaveRequest.Status = "Rejected";
            leaveRequest.ApprovedDate = DateTime.Now;
            leaveRequest.ApprovedBy = "Admin";

            await _context.SaveChangesAsync();

            TempData["Success"] = "Leave request rejected successfully.";

            return RedirectToAction(nameof(Index));
        }

        // ==========================
        // Load Dropdowns
        // ==========================
        private void LoadDropdowns()
        {
            ViewBag.EmployeeId = new SelectList(
                _context.Employees,
                "EmployeeId",
                "FirstName");

            ViewBag.LeaveTypeId = new SelectList(
                _context.LeaveTypes,
                "LeaveTypeId",
                "LeaveName");
        }
    }
}
