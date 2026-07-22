using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagement.Controllers
{
    public class LeaveRequestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
