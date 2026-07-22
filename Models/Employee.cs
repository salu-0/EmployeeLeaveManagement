using EmployeeLeaveManagement.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeCode { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Designation { get; set; }

    public DateTime JoiningDate { get; set; }

    public bool IsActive { get; set; }

    public string? ProfileImage { get; set; }

    public int DepartmentId { get; set; }

    public Department Department { get; set; }
}