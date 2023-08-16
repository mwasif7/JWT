using JWT.Api.Models;
using JWT.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Api.Controllers;

[ApiController]
[Route("api/{controller}")]
public class EmployeeController: ControllerBase
{
    private readonly IEmployees _emp;

    public EmployeeController(IEmployees emp)
  {
    _emp = emp;
  }

[HttpGet]
  public async Task<IEnumerable<Employee>> Get()
  {
    return await Task.FromResult(_emp.GetAllEmployeeDetails());
  }

  [HttpGet("{id}")]
  [Authorize]
  public async Task<ActionResult<Employee>> GetEmployeeById(int id)
  {
    var employees = await Task.FromResult(_emp.GetEmployeeDetails(id));
    if (employees == null)
    {
      return NotFound();
    }
    return employees;
  }

  [HttpPost]
  [Authorize]
  public async Task<IActionResult> Post(Employee employee)
  {
    var result = await _emp.AddEmployee(employee);
    if(result == Enums.EmployeeResults.Success)
      return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeID }, employee);
    return BadRequest(result);

  }

  [HttpPut("{id}")]
  [Authorize]
  public async Task<IActionResult> Put(int id, Employee employee)
  {
    if(id != employee.EmployeeID)
      return BadRequest();
   var result = await _emp.UpdateEmployee(employee);
   if(result == Enums.EmployeeResults.Success)
       return Ok("Data updated");
    return BadRequest();

  }

  [HttpDelete("{id}")]
  [Authorize]
  public async Task<IActionResult> Delete(int id)
  {
     var result =  await _emp.DeleteEmployee(id);
     if(result == Enums.EmployeeResults.Success)
        return Ok("Data deleted");
      return BadRequest(); 
  }
}