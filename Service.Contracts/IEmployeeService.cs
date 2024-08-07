using Entities.Models;
using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges);
    EmployeeDto CreateEmployee(Guid companyId, EmployeeForCreationDto employee, bool trackChanges);
    void DeleteEmployee(Guid companyId, Guid id, bool trackChanges);
    void UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employeeForUpdate, 
        bool compTrackChanges, bool empTrackChanges);
    (EmployeeForUpdateDto employeeToPatch, Employee employee) GetEmployeeForPatch(Guid companyId, Guid id,
        bool compTrackChanges, bool empTrackChanges);
    void SaveChangesForPatch(EmployeeForUpdateDto employeeToPatch, Employee employee);
}
