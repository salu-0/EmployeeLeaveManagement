using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================
        // Employee List
        // ==========================
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees
                .Include(e => e.Department)
                .ToListAsync();

            return View(employees);
        }

        // ==========================
        // Create Employee (GET)
        // ==========================
        public IActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName");

            return View();
        }

        // ==========================
        // Create Employee (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Employee added successfully.";

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            return View(employee);
        }

        // ==========================
        // Employee Details
        // ==========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // ==========================
        // Edit Employee (GET)
        // ==========================

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound();

            ViewBag.DepartmentId = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            return View(employee);
        }

        // ==========================
        // Edit Employee (POST)
        // ==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Employee updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.DepartmentId = new SelectList(
                _context.Departments,
                "DepartmentId",
                "DepartmentName",
                employee.DepartmentId);

            return View(employee);
        }

        // ==========================
        // Delete Employee (GET)
        // ==========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == id);

            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // ==========================
        // Delete Employee (POST)
        // ==========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Employee deleted successfully.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Employee not found."
                });
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Employee deleted successfully."
            });
        }
    }
}