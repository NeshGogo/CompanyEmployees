using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;


[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _sender;
    public CompaniesController(ISender sender) => _sender = sender; 
    
    [HttpGet] 
    public async Task<IActionResult> GetCompanies()
    {
        var companies = await _sender.Send(new GetCompaniesQuery(TrackChanges: false));

        return Ok(companies);
    }

    [HttpGet("{id:guid}", Name = "GetCompanyById")]
    public async Task<IActionResult> GetCompanyById(Guid id)
    {
        var company = await _sender.Send(new GetCompanyQuery(id, TrackChanges: false));

        return Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto companyForCreation)
    {
        if (companyForCreation is null)
            return BadRequest("CompanyForCreationDto object is null");

        var company = await _sender.Send(new CreateCompanyCommand(companyForCreation));

        return CreatedAtRoute("GetCompanyById", new { id = company.Id }, company);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCompany(Guid id, CompanyForUpdateDto companyForUpdate)
    {
        if (companyForUpdate is null)
            return BadRequest("CompanyForUpdateDto object is null");

        await _sender.Send(new UpdateCompanyCommand(id, companyForUpdate, TrackChanges: true));

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _sender.Send(new DeleteCompanyCommand(id, TrackChanges: false));

        return NoContent();
    }
}
