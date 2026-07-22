using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagement.Controllers
{
    public class LeaveTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
