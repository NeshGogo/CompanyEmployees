using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Presentation.Controllers;


[Route("api/companies")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ISender _sender;
    public CompaniesController(ISender sender) => _sender = sender; 
}
