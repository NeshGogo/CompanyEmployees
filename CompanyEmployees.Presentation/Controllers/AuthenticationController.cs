using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using CompanyEmployees.Presentation.ActionFilters;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IServiceManager _service;

    public AuthenticationController(IServiceManager service) => _service = service;

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto user)
    {
        var result = await _service.AuthenticationService.RegisterUser(user);
        
        if(!result.Succeeded) 
        {
            foreach(var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return StatusCode(201);
    }

    [HttpPost("Login")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Autheticate([FromBody] UserForAuthenticationDto user)
    {
        if(!await _service.AuthenticationService.ValidateUser(user))
            return Unauthorized();

        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

        return Ok(tokenDto);
    }
}
