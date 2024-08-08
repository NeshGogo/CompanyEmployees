﻿using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransferObjects;

public record EmployeeForCreationDto(
    [Required(ErrorMessage = "Employee name is a required field")]
    [MaxLength(30, ErrorMessage = "Maximun length for the Name is 30 characters.")]
    string Name,
    [Required(ErrorMessage = "Employee age is a required field")]
    [Range(18, int.MaxValue, ErrorMessage = "Age is required and it can't be lower than 18")]
    int Age,
    [Required(ErrorMessage = "Employee position is a required field")]
    [MaxLength(30, ErrorMessage = "Maximun length for the Position is 20 characters.")]
    string Position
);
