using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                // Dashboard Statistics
                TotalEmployees = await _context.Employees.CountAsync(),

                TotalDepartments = await _context.Departments.CountAsync(),

                TotalLeaveTypes = await _context.LeaveTypes.CountAsync(),

                PendingLeaveRequests = await _context.LeaveRequests
                    .CountAsync(l => l.Status == "Pending")
            };

            // Employee Count by Department (Optimized)
            dashboard.DepartmentChart = await _context.Departments
                .GroupJoin(
                    _context.Employees,
                    department => department.DepartmentId,
                    employee => employee.DepartmentId,
                    (department, employees) => new DepartmentChartViewModel
                    {
                        DepartmentName = department.DepartmentName,
                        EmployeeCount = employees.Count()
                    })
                .ToListAsync();

            // Recent Employees
            dashboard.RecentEmployees = await _context.Employees
                .Include(e => e.Department)
                .OrderByDescending(e => e.EmployeeId)
                .Take(5)
                .ToListAsync();

            return View(dashboard);
        }
    }
}