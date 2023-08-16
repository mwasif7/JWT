using JWT.Api.Enums;
using JWT.Api.Models;

namespace JWT.Api.Repository;
  public interface IEmployees
  {
    public IEnumerable<Employee> GetAllEmployeeDetails();
    public Employee? GetEmployeeDetails(int id);
    public Task<EmployeeResults> AddEmployee(Employee employee);
    public Task<EmployeeResults> UpdateEmployee(Employee employee);
    public Task<EmployeeResults> DeleteEmployee(int id);
  }