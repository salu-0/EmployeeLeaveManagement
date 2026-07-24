using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalEmployees { get; set; }

        public int TotalDepartments { get; set; }

        public int TotalLeaveTypes { get; set; }

        public int PendingLeaveRequests { get; set; }

        public List<DepartmentChartViewModel> DepartmentChart { get; set; } = new();

        public List<Employee> RecentEmployees { get; set; } = new();
    }
}