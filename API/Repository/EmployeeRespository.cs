using System.Linq.Expressions;
using JWT.Api.Data;
using JWT.Api.Enums;
using JWT.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT.Api.Repository;


public class EmployeeRepository : IEmployees
{
  private readonly DatabaseContext _dbContext;

  public EmployeeRepository(DatabaseContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IEnumerable<Employee> GetAllEmployeeDetails()
  {
    try
    {
      return _dbContext.Employees.ToList();
    }
    catch
    {
      throw;
    }
  }

  public Employee? GetEmployeeDetails(int id)
  {
    try
    {
      return _dbContext.Employees.Where(x => x.EmployeeID == id).FirstOrDefault();
    }
    catch
    {
      throw;
    }
  }

  public async Task<EmployeeResults> AddEmployee(Employee employee)
  {
    try
    {
      var isEmployeeExists = GetEmployeeDetails(employee.EmployeeID);
      if (isEmployeeExists is null)
      {
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
        return EmployeeResults.Success;
      }
      return EmployeeResults.EmployeeExists;
    }
    catch
    {
      throw;
    }
  }

  public async Task<EmployeeResults> UpdateEmployee(Employee employee)
  {
    try
    {
      _dbContext.Entry(employee).State = EntityState.Modified;
      await _dbContext.SaveChangesAsync();
      return EmployeeResults.Success;
    }
    catch
    {
      throw;
    }
  }

  public async Task<EmployeeResults> DeleteEmployee(int id)
  {
    try
    {
      var employee = GetEmployeeDetails(id);
      if (employee is not null)
      {
        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();
        return EmployeeResults.Success;
      }
      return EmployeeResults.Error;
    }

    catch
    {
      throw;
    }
  }

}