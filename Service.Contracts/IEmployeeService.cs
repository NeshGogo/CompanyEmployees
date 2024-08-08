﻿using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeAsync(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    Task DeleteEmployeeAsync(Guid companyId, Guid id, bool trackChanges);
    Task UpdateEmployeeAsync(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, 
        bool compTrackChanges, bool empTrackChanges);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employee)> GetEmployeeForPatchAsync(Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges);
    Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employee);
}
