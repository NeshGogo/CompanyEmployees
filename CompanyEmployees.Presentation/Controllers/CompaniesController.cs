using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    
}
