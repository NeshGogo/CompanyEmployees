using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/Companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;

    public EmployeesController(IServiceManager service) => _service = service;

    [HttpGet]
    public IActionResult GetEmployeesForCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployee(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id, trackChanges: false);
        return Ok(employee);
    }

    [HttpPost]
    public IActionResult CreateEmployee(Guid companyId, EmployeeForCreationDto employee)
    {
        if (employee == null)
            return BadRequest("EmployeeForCreationDto object is null");

        var employeeDto = _service.EmployeeService.CreateEmployee(companyId, employee, trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeDto.Id }, employeeDto);
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployee(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployee(companyId, id, trackChanges: false);
        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployee(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForUpdateDto object is null");

        _service.EmployeeService.UpdateEmployee(companyId, id, employee, 
                compTrackChanges: false, empTrackChanges: true);

        return NoContent();
    }
}
