﻿using Entities.Models;

namespace Entities.Exceptions;

public class RoleNotFoundException : NotFoundException
{
    public RoleNotFoundException(IEnumerable<string> roles) 
        : base($"Roles with names: {string.Join(',', roles)} doesn't exist in the database")
    {
    }
}
