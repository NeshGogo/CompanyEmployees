using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILoggerManager logger, IMapper mapper, 
        UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
        IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userCreateDto)
    {
        var user = _mapper.Map<User>(userCreateDto);

        var result = await _userManager.CreateAsync(user, userCreateDto.Password);

        if (result.Succeeded)
        {
            if(userCreateDto.Roles is not null)
            {
                var notExistsRoles = new List<string>();
                
                foreach (var role in userCreateDto.Roles)
                {
                    var exists = await _roleManager.RoleExistsAsync(role);
                    if (!exists)
                        notExistsRoles.Add(role);
                }
                
                if (notExistsRoles.Any())
                    throw new RoleNotFoundException(notExistsRoles);
               
                await _userManager.AddToRolesAsync(user, userCreateDto.Roles);
            }
        }            

        return result;
    }
}
